
/*
* UI overlay class using ImGui
*
* Copyright (C) 2017 by Sascha Willems - www.saschawillems.de
*
* This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/

#include "VulkanUIOverlay.h"

namespace vks
{
    UIOverlay::UIOverlay()
    {
        _drawCommandBuffer = VK_NULL_HANDLE;

        // Init ImGui
        ImGui::CreateContext();
        // Color scheme
        ImGuiStyle& style = ImGui::GetStyle();
        style.Colors[ImGuiCol_TitleBg] = ImVec4(1.0f, 0.0f, 0.0f, 1.0f);
        style.Colors[ImGuiCol_TitleBgActive] = ImVec4(1.0f, 0.0f, 0.0f, 1.0f);
        style.Colors[ImGuiCol_TitleBgCollapsed] = ImVec4(1.0f, 0.0f, 0.0f, 0.1f);
        style.Colors[ImGuiCol_MenuBarBg] = ImVec4(1.0f, 0.0f, 0.0f, 0.4f);
        style.Colors[ImGuiCol_Header] = ImVec4(0.8f, 0.0f, 0.0f, 0.4f);
        style.Colors[ImGuiCol_HeaderActive] = ImVec4(1.0f, 0.0f, 0.0f, 0.4f);
        style.Colors[ImGuiCol_HeaderHovered] = ImVec4(1.0f, 0.0f, 0.0f, 0.4f);
        style.Colors[ImGuiCol_FrameBg] = ImVec4(0.0f, 0.0f, 0.0f, 0.8f);
        style.Colors[ImGuiCol_CheckMark] = ImVec4(1.0f, 0.0f, 0.0f, 0.8f);
        style.Colors[ImGuiCol_SliderGrab] = ImVec4(1.0f, 0.0f, 0.0f, 0.4f);
        style.Colors[ImGuiCol_SliderGrabActive] = ImVec4(1.0f, 0.0f, 0.0f, 0.8f);
        style.Colors[ImGuiCol_FrameBgHovered] = ImVec4(1.0f, 1.0f, 1.0f, 0.1f);
        style.Colors[ImGuiCol_FrameBgActive] = ImVec4(1.0f, 1.0f, 1.0f, 0.2f);
        style.Colors[ImGuiCol_Button] = ImVec4(1.0f, 0.0f, 0.0f, 0.4f);
        style.Colors[ImGuiCol_ButtonHovered] = ImVec4(1.0f, 0.0f, 0.0f, 0.6f);
        style.Colors[ImGuiCol_ButtonActive] = ImVec4(1.0f, 0.0f, 0.0f, 0.8f);
        // Dimensions
        ImGuiIO& io = ImGui::GetIO();
        io.FontGlobalScale = scale;
    }

    UIOverlay::~UIOverlay()
    {
    }

    uint32_t getMemoryType(VkPhysicalDeviceMemoryProperties memoryProperties, uint32_t typeBits, VkMemoryPropertyFlags properties)
    {
        for (uint32_t i = 0; i < memoryProperties.memoryTypeCount; i++)
        {
            if ((typeBits & 1) == 1)
            {
                if ((memoryProperties.memoryTypes[i].propertyFlags & properties) == properties)
                {
                    return i;
                }
            }
            typeBits >>= 1;
        }

        throw std::runtime_error("Could not find a matching memory type");
    }

    VkResult UIOverlay::createBuffer(VkBufferUsageFlags usageFlags, VkMemoryPropertyFlags memoryPropertyFlags, vks::Buffer* buffer, VkDeviceSize size, void* data)
    {
        buffer->device = _device; 

        // Create the buffer handle
        VkBufferCreateInfo bufferCreateInfo = vks::initializers::bufferCreateInfo(usageFlags, size);
        VK_CHECK_RESULT(vkCreateBuffer(_device, &bufferCreateInfo, nullptr, &buffer->buffer));

        // Create the memory backing up the buffer handle
        VkMemoryRequirements memReqs;
        VkMemoryAllocateInfo memAlloc = vks::initializers::memoryAllocateInfo();
        vkGetBufferMemoryRequirements(_device, buffer->buffer, &memReqs);
        memAlloc.allocationSize = memReqs.size;
        // Find a memory type index that fits the properties of the buffer
        memAlloc.memoryTypeIndex = getMemoryType(_memoryProperties, memReqs.memoryTypeBits, memoryPropertyFlags);
        // If the buffer has VK_BUFFER_USAGE_SHADER_DEVICE_ADDRESS_BIT set we also need to enable the appropriate flag during allocation
        VkMemoryAllocateFlagsInfoKHR allocFlagsInfo{};
        if (usageFlags & VK_BUFFER_USAGE_SHADER_DEVICE_ADDRESS_BIT) {
            allocFlagsInfo.sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_FLAGS_INFO_KHR;
            allocFlagsInfo.flags = VK_MEMORY_ALLOCATE_DEVICE_ADDRESS_BIT_KHR;
            memAlloc.pNext = &allocFlagsInfo;
        }
        VK_CHECK_RESULT(vkAllocateMemory(_device, &memAlloc, nullptr, &buffer->memory));

        buffer->alignment = memReqs.alignment;
        buffer->size = size;
        buffer->usageFlags = usageFlags;
        buffer->memoryPropertyFlags = memoryPropertyFlags;

        // If a pointer to the buffer data has been passed, map the buffer and copy over the data
        if (data != nullptr)
        {
            VK_CHECK_RESULT(buffer->map());
            memcpy(buffer->mapped, data, size);
            if ((memoryPropertyFlags & VK_MEMORY_PROPERTY_HOST_COHERENT_BIT) == 0)
                buffer->flush();

            buffer->unmap();
        }

        // Initialize a default descriptor that covers the whole buffer size
        buffer->setupDescriptor();

        // Attach the memory to the buffer object
        return buffer->bind();
    }

    VkCommandBuffer UIOverlay::createCommandBuffer(VkCommandBufferLevel level, VkCommandPool pool, bool begin)
    {
        VkCommandBufferAllocateInfo cmdBufAllocateInfo = vks::initializers::commandBufferAllocateInfo(pool, level, 1);
        VkCommandBuffer cmdBuffer;
        VK_CHECK_RESULT(vkAllocateCommandBuffers(_device, &cmdBufAllocateInfo, &cmdBuffer));
        // If requested, also start recording for the new command buffer
        if (begin)
        {
            VkCommandBufferBeginInfo cmdBufInfo = vks::initializers::commandBufferBeginInfo();
            VK_CHECK_RESULT(vkBeginCommandBuffer(cmdBuffer, &cmdBufInfo));
        }
        return cmdBuffer;
    }

    void UIOverlay::flushCommandBuffer(VkCommandBuffer commandBuffer, VkQueue queue, VkCommandPool pool, bool free)
    {
        if (commandBuffer == VK_NULL_HANDLE)
        {
            return;
        }

        VK_CHECK_RESULT(vkEndCommandBuffer(commandBuffer));

        VkSubmitInfo submitInfo = vks::initializers::submitInfo();
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers = &commandBuffer;
        // Create fence to ensure that the command buffer has finished executing
        VkFenceCreateInfo fenceInfo = vks::initializers::fenceCreateInfo(VK_FLAGS_NONE);
        VkFence fence;
        VK_CHECK_RESULT(vkCreateFence(_device, &fenceInfo, nullptr, &fence));
        // Submit to the queue
        VK_CHECK_RESULT(vkQueueSubmit(queue, 1, &submitInfo, fence));
        // Wait for the fence to signal that command buffer has finished executing
        VK_CHECK_RESULT(vkWaitForFences(_device, 1, &fence, VK_TRUE, DEFAULT_FENCE_TIMEOUT));
        vkDestroyFence(_device, fence, nullptr);
        if (free)
        {
            vkFreeCommandBuffers(_device, pool, 1, &commandBuffer);
        }
    }
    /** Prepare all vulkan resources required to render the UI overlay */
    void UIOverlay::prepareResources(VkQueue queue, VkCommandPool pool)
    {
        ImGuiIO& io = ImGui::GetIO();

        // Create font texture
        unsigned char* fontData;
        int texWidth, texHeight;
        const std::string filename = "../../runtime/Roboto-Medium.ttf";
        io.Fonts->AddFontFromFileTTF(filename.c_str(), 16.0f);
        io.Fonts->GetTexDataAsRGBA32(&fontData, &texWidth, &texHeight);
        VkDeviceSize uploadSize = texWidth * texHeight * 4 * sizeof(char);

        // Create target image for copy
        VkImageCreateInfo imageInfo = vks::initializers::imageCreateInfo();
        imageInfo.imageType = VK_IMAGE_TYPE_2D;
        imageInfo.format = VK_FORMAT_R8G8B8A8_UNORM;
        imageInfo.extent.width = texWidth;
        imageInfo.extent.height = texHeight;
        imageInfo.extent.depth = 1;
        imageInfo.mipLevels = 1;
        imageInfo.arrayLayers = 1;
        imageInfo.samples = VK_SAMPLE_COUNT_1_BIT;
        imageInfo.tiling = VK_IMAGE_TILING_OPTIMAL;
        imageInfo.usage = VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT;
        imageInfo.sharingMode = VK_SHARING_MODE_EXCLUSIVE;
        imageInfo.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED;
        VK_CHECK_RESULT(vkCreateImage(_device, &imageInfo, nullptr, &fontImage));
        VkMemoryRequirements memReqs;
        vkGetImageMemoryRequirements(_device, fontImage, &memReqs);
        VkMemoryAllocateInfo memAllocInfo = vks::initializers::memoryAllocateInfo();
        memAllocInfo.allocationSize = memReqs.size;
        memAllocInfo.memoryTypeIndex = getMemoryType(_memoryProperties, memReqs.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT);
        VK_CHECK_RESULT(vkAllocateMemory(_device, &memAllocInfo, nullptr, &fontMemory));
        VK_CHECK_RESULT(vkBindImageMemory(_device, fontImage, fontMemory, 0));

        // Image view
        VkImageViewCreateInfo viewInfo = vks::initializers::imageViewCreateInfo();
        viewInfo.image = fontImage;
        viewInfo.viewType = VK_IMAGE_VIEW_TYPE_2D;
        viewInfo.format = VK_FORMAT_R8G8B8A8_UNORM;
        viewInfo.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
        viewInfo.subresourceRange.levelCount = 1;
        viewInfo.subresourceRange.layerCount = 1;
        VK_CHECK_RESULT(vkCreateImageView(_device, &viewInfo, nullptr, &fontView));

        // Staging buffers for font data upload
        vks::Buffer stagingBuffer;

        VK_CHECK_RESULT(createBuffer(
            VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
            VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT,
            &stagingBuffer,
            uploadSize));

        stagingBuffer.map();
        memcpy(stagingBuffer.mapped, fontData, uploadSize);
        stagingBuffer.unmap();

        // Copy buffer data to font image
        VkCommandBuffer copyCmd = createCommandBuffer(VK_COMMAND_BUFFER_LEVEL_PRIMARY, pool, true);

        // Prepare for transfer
        vks::tools::setImageLayout(
            copyCmd,
            fontImage,
            VK_IMAGE_ASPECT_COLOR_BIT,
            VK_IMAGE_LAYOUT_UNDEFINED,
            VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
            VK_PIPELINE_STAGE_HOST_BIT,
            VK_PIPELINE_STAGE_TRANSFER_BIT);

        // Copy
        VkBufferImageCopy bufferCopyRegion = {};
        bufferCopyRegion.imageSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
        bufferCopyRegion.imageSubresource.layerCount = 1;
        bufferCopyRegion.imageExtent.width = texWidth;
        bufferCopyRegion.imageExtent.height = texHeight;
        bufferCopyRegion.imageExtent.depth = 1;

        vkCmdCopyBufferToImage(
            copyCmd,
            stagingBuffer.buffer,
            fontImage,
            VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
            1,
            &bufferCopyRegion
        );

        // Prepare for shader read
        vks::tools::setImageLayout(
            copyCmd,
            fontImage,
            VK_IMAGE_ASPECT_COLOR_BIT,
            VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
            VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
            VK_PIPELINE_STAGE_TRANSFER_BIT,
            VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT);

        flushCommandBuffer(copyCmd, queue, pool, true);

        stagingBuffer.destroy();

        // Font texture Sampler
        VkSamplerCreateInfo samplerInfo = vks::initializers::samplerCreateInfo();
        samplerInfo.magFilter = VK_FILTER_LINEAR;
        samplerInfo.minFilter = VK_FILTER_LINEAR;
        samplerInfo.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR;
        samplerInfo.addressModeU = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
        samplerInfo.addressModeV = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
        samplerInfo.addressModeW = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
        samplerInfo.borderColor = VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE;
        VK_CHECK_RESULT(vkCreateSampler(_device, &samplerInfo, nullptr, &sampler));

        // Descriptor pool
        std::vector<VkDescriptorPoolSize> poolSizes = {
            vks::initializers::descriptorPoolSize(VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, 1)
        };
        VkDescriptorPoolCreateInfo descriptorPoolInfo = vks::initializers::descriptorPoolCreateInfo(poolSizes, 2);
        VK_CHECK_RESULT(vkCreateDescriptorPool(_device, &descriptorPoolInfo, nullptr, &descriptorPool));

        // Descriptor set layout
        std::vector<VkDescriptorSetLayoutBinding> setLayoutBindings = {
            vks::initializers::descriptorSetLayoutBinding(VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, VK_SHADER_STAGE_FRAGMENT_BIT, 0),
        };
        VkDescriptorSetLayoutCreateInfo descriptorLayout = vks::initializers::descriptorSetLayoutCreateInfo(setLayoutBindings);
        VK_CHECK_RESULT(vkCreateDescriptorSetLayout(_device, &descriptorLayout, nullptr, &descriptorSetLayout));

        // Descriptor set
        VkDescriptorSetAllocateInfo allocInfo = vks::initializers::descriptorSetAllocateInfo(descriptorPool, &descriptorSetLayout, 1);
        VK_CHECK_RESULT(vkAllocateDescriptorSets(_device, &allocInfo, &descriptorSet));
        VkDescriptorImageInfo fontDescriptor = vks::initializers::descriptorImageInfo(
            sampler,
            fontView,
            VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
        );
        std::vector<VkWriteDescriptorSet> writeDescriptorSets = {
            vks::initializers::writeDescriptorSet(descriptorSet, VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, 0, &fontDescriptor)
        };
        vkUpdateDescriptorSets(_device, static_cast<uint32_t>(writeDescriptorSets.size()), writeDescriptorSets.data(), 0, nullptr);
    }

    /** Prepare a separate pipeline for the UI overlay rendering decoupled from the main application */
    void UIOverlay::preparePipeline(const VkRenderPass renderPass)
    {
        VkPipelineCacheCreateInfo pipelineCacheCreateInfo = {};
        pipelineCacheCreateInfo.sType = VK_STRUCTURE_TYPE_PIPELINE_CACHE_CREATE_INFO;
        VK_CHECK_RESULT(vkCreatePipelineCache(_device, &pipelineCacheCreateInfo, nullptr, &_pipelineCache));

        // Pipeline layout
        // Push constants for UI rendering parameters
        VkPushConstantRange pushConstantRange = vks::initializers::pushConstantRange(VK_SHADER_STAGE_VERTEX_BIT, sizeof(PushConstBlock), 0);
        VkPipelineLayoutCreateInfo pipelineLayoutCreateInfo = vks::initializers::pipelineLayoutCreateInfo(&descriptorSetLayout, 1);
        pipelineLayoutCreateInfo.pushConstantRangeCount = 1;
        pipelineLayoutCreateInfo.pPushConstantRanges = &pushConstantRange;
        VK_CHECK_RESULT(vkCreatePipelineLayout(_device, &pipelineLayoutCreateInfo, nullptr, &pipelineLayout));

        // Setup graphics pipeline for UI rendering
        VkPipelineInputAssemblyStateCreateInfo inputAssemblyState =
            vks::initializers::pipelineInputAssemblyStateCreateInfo(VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST, 0, VK_FALSE);

        VkPipelineRasterizationStateCreateInfo rasterizationState =
            vks::initializers::pipelineRasterizationStateCreateInfo(VK_POLYGON_MODE_FILL, VK_CULL_MODE_NONE, VK_FRONT_FACE_COUNTER_CLOCKWISE);

        // Enable blending
        VkPipelineColorBlendAttachmentState blendAttachmentState{};
        blendAttachmentState.blendEnable = VK_TRUE;
        blendAttachmentState.colorWriteMask = VK_COLOR_COMPONENT_R_BIT | VK_COLOR_COMPONENT_G_BIT | VK_COLOR_COMPONENT_B_BIT | VK_COLOR_COMPONENT_A_BIT;
        blendAttachmentState.srcColorBlendFactor = VK_BLEND_FACTOR_SRC_ALPHA;
        blendAttachmentState.dstColorBlendFactor = VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA;
        blendAttachmentState.colorBlendOp = VK_BLEND_OP_ADD;
        blendAttachmentState.srcAlphaBlendFactor = VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA;
        blendAttachmentState.dstAlphaBlendFactor = VK_BLEND_FACTOR_ZERO;
        blendAttachmentState.alphaBlendOp = VK_BLEND_OP_ADD;

        VkPipelineColorBlendStateCreateInfo colorBlendState =
            vks::initializers::pipelineColorBlendStateCreateInfo(1, &blendAttachmentState);

        VkPipelineDepthStencilStateCreateInfo depthStencilState =
            vks::initializers::pipelineDepthStencilStateCreateInfo(VK_FALSE, VK_FALSE, VK_COMPARE_OP_ALWAYS);

        VkPipelineViewportStateCreateInfo viewportState =
            vks::initializers::pipelineViewportStateCreateInfo(1, 1, 0);

        VkPipelineMultisampleStateCreateInfo multisampleState =
            vks::initializers::pipelineMultisampleStateCreateInfo(rasterizationSamples);

        std::vector<VkDynamicState> dynamicStateEnables = {
            VK_DYNAMIC_STATE_VIEWPORT,
            VK_DYNAMIC_STATE_SCISSOR
        };
        VkPipelineDynamicStateCreateInfo dynamicState =
            vks::initializers::pipelineDynamicStateCreateInfo(dynamicStateEnables);

        VkGraphicsPipelineCreateInfo pipelineCreateInfo = vks::initializers::pipelineCreateInfo(pipelineLayout, renderPass);

        pipelineCreateInfo.pInputAssemblyState = &inputAssemblyState;
        pipelineCreateInfo.pRasterizationState = &rasterizationState;
        pipelineCreateInfo.pColorBlendState = &colorBlendState;
        pipelineCreateInfo.pMultisampleState = &multisampleState;
        pipelineCreateInfo.pViewportState = &viewportState;
        pipelineCreateInfo.pDepthStencilState = &depthStencilState;
        pipelineCreateInfo.pDynamicState = &dynamicState;
        pipelineCreateInfo.stageCount = static_cast<uint32_t>(shaders.size());
        pipelineCreateInfo.pStages = shaders.data();
        pipelineCreateInfo.subpass = subpass;

        // Vertex bindings an attributes based on ImGui vertex definition
        std::vector<VkVertexInputBindingDescription> vertexInputBindings = {
            vks::initializers::vertexInputBindingDescription(0, sizeof(ImDrawVert), VK_VERTEX_INPUT_RATE_VERTEX),
        };
        std::vector<VkVertexInputAttributeDescription> vertexInputAttributes = {
            vks::initializers::vertexInputAttributeDescription(0, 0, VK_FORMAT_R32G32_SFLOAT, offsetof(ImDrawVert, pos)),	// Location 0: Position
            vks::initializers::vertexInputAttributeDescription(0, 1, VK_FORMAT_R32G32_SFLOAT, offsetof(ImDrawVert, uv)),	// Location 1: UV
            vks::initializers::vertexInputAttributeDescription(0, 2, VK_FORMAT_R8G8B8A8_UNORM, offsetof(ImDrawVert, col)),	// Location 0: Color
        };
        VkPipelineVertexInputStateCreateInfo vertexInputState = vks::initializers::pipelineVertexInputStateCreateInfo();
        vertexInputState.vertexBindingDescriptionCount = static_cast<uint32_t>(vertexInputBindings.size());
        vertexInputState.pVertexBindingDescriptions = vertexInputBindings.data();
        vertexInputState.vertexAttributeDescriptionCount = static_cast<uint32_t>(vertexInputAttributes.size());
        vertexInputState.pVertexAttributeDescriptions = vertexInputAttributes.data();

        pipelineCreateInfo.pVertexInputState = &vertexInputState;

        VK_CHECK_RESULT(vkCreateGraphicsPipelines(_device, _pipelineCache, 1, &pipelineCreateInfo, nullptr, &pipeline));
    }

    /** Update vertex and index buffer containing the imGui elements when required */
    bool UIOverlay::update()
    {
        ImDrawData* imDrawData = ImGui::GetDrawData();
        bool updateCmdBuffers = false;

        if (!imDrawData) { return updateCmdBuffers; };

        // Note: Alignment is done inside buffer creation
        VkDeviceSize vertexBufferSize = imDrawData->TotalVtxCount * sizeof(ImDrawVert);
        VkDeviceSize indexBufferSize = imDrawData->TotalIdxCount * sizeof(ImDrawIdx);

        // Update buffers only if vertex or index count has been changed compared to current buffer size
        if ((vertexBufferSize == 0) || (indexBufferSize == 0)) {
            return updateCmdBuffers;
        }

        // Vertex buffer
        if ((vertexBuffer.buffer == VK_NULL_HANDLE) || (vertexCount != imDrawData->TotalVtxCount)) {
            // abort if we are not on the same command buffer that we were drawing on since this will cause a problem
            // freeing a buffer since it may be in use still
            //if (_drawCommandBuffer != commandBuffer)
            //    return updateCmdBuffers;

            vertexBuffer.unmap();
            vertexBuffer.destroy();
            VK_CHECK_RESULT(createBuffer(VK_BUFFER_USAGE_VERTEX_BUFFER_BIT, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT, &vertexBuffer, vertexBufferSize));
            vertexCount = imDrawData->TotalVtxCount;
            vertexBuffer.unmap();
            vertexBuffer.map();
            updateCmdBuffers = true;
        }

        // Index buffer
        VkDeviceSize indexSize = imDrawData->TotalIdxCount * sizeof(ImDrawIdx);
        if ((indexBuffer.buffer == VK_NULL_HANDLE) || (indexCount < imDrawData->TotalIdxCount)) {
            indexBuffer.unmap();
            indexBuffer.destroy();
            VK_CHECK_RESULT(createBuffer(VK_BUFFER_USAGE_INDEX_BUFFER_BIT, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT, &indexBuffer, indexBufferSize));
            indexCount = imDrawData->TotalIdxCount;
            indexBuffer.map();
            updateCmdBuffers = true;
        }

        // Upload data
        ImDrawVert* vtxDst = (ImDrawVert*)vertexBuffer.mapped;
        ImDrawIdx* idxDst = (ImDrawIdx*)indexBuffer.mapped;

        for (int n = 0; n < imDrawData->CmdListsCount; n++) {
            const ImDrawList* cmd_list = imDrawData->CmdLists[n];
            memcpy(vtxDst, cmd_list->VtxBuffer.Data, cmd_list->VtxBuffer.Size * sizeof(ImDrawVert));
            memcpy(idxDst, cmd_list->IdxBuffer.Data, cmd_list->IdxBuffer.Size * sizeof(ImDrawIdx));
            vtxDst += cmd_list->VtxBuffer.Size;
            idxDst += cmd_list->IdxBuffer.Size;
        }

        // Flush to make writes visible to GPU
        vertexBuffer.flush();
        indexBuffer.flush();

        return updateCmdBuffers;
    }

    void UIOverlay::draw(VkCommandBuffer commandBuffer)
    {
        _drawCommandBuffer = commandBuffer;

        ImDrawData* imDrawData = ImGui::GetDrawData();
        int32_t vertexOffset = 0;
        int32_t indexOffset = 0;

        if ((!imDrawData) || (imDrawData->CmdListsCount == 0)) {
            return;
        }

        ImGuiIO& io = ImGui::GetIO();

        vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
        vkCmdBindDescriptorSets(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, &descriptorSet, 0, NULL);

        pushConstBlock.scale = glm::vec2(2.0f / io.DisplaySize.x, 2.0f / io.DisplaySize.y);
        pushConstBlock.translate = glm::vec2(-1.0f);
        vkCmdPushConstants(commandBuffer, pipelineLayout, VK_SHADER_STAGE_VERTEX_BIT, 0, sizeof(PushConstBlock), &pushConstBlock);

        VkDeviceSize offsets[1] = { 0 };
        vkCmdBindVertexBuffers(commandBuffer, 0, 1, &vertexBuffer.buffer, offsets);
        vkCmdBindIndexBuffer(commandBuffer, indexBuffer.buffer, 0, VK_INDEX_TYPE_UINT16);

        for (int32_t i = 0; i < imDrawData->CmdListsCount; i++)
        {
            const ImDrawList* cmd_list = imDrawData->CmdLists[i];
            for (int32_t j = 0; j < cmd_list->CmdBuffer.Size; j++)
            {
                const ImDrawCmd* pcmd = &cmd_list->CmdBuffer[j];
                VkRect2D scissorRect;
                scissorRect.offset.x = std::max((int32_t)(pcmd->ClipRect.x), 0);
                scissorRect.offset.y = std::max((int32_t)(pcmd->ClipRect.y), 0);
                scissorRect.extent.width = (uint32_t)(pcmd->ClipRect.z - pcmd->ClipRect.x);
                scissorRect.extent.height = (uint32_t)(pcmd->ClipRect.w - pcmd->ClipRect.y);
                vkCmdSetScissor(commandBuffer, 0, 1, &scissorRect);
                vkCmdDrawIndexed(commandBuffer, pcmd->ElemCount, 1, indexOffset, vertexOffset, 0);
                indexOffset += pcmd->ElemCount;
            }
            vertexOffset += cmd_list->VtxBuffer.Size;
        }
    }

    void UIOverlay::resize(uint32_t width, uint32_t height)
    {
        ImGuiIO& io = ImGui::GetIO();
        io.DisplaySize = ImVec2((float)(width), (float)(height));
    }

    void UIOverlay::freeResources()
    {
        ImGui::DestroyContext();
        vertexBuffer.destroy();
        indexBuffer.destroy();
        vkDestroyImageView(_device, fontView, nullptr);
        vkDestroyImage(_device, fontImage, nullptr);
        vkFreeMemory(_device, fontMemory, nullptr);
        vkDestroySampler(_device, sampler, nullptr);
        vkDestroyDescriptorSetLayout(_device, descriptorSetLayout, nullptr);
        vkDestroyDescriptorPool(_device, descriptorPool, nullptr);
        vkDestroyPipelineLayout(_device, pipelineLayout, nullptr);
        vkDestroyPipeline(_device, pipeline, nullptr);
        for (auto& shaderStageCreateInfo : shaders)
        {
            vkDestroyShaderModule(_device, shaderStageCreateInfo.module, nullptr);
        }
        vkDestroyPipelineCache(_device, _pipelineCache, nullptr);

    }

    bool UIOverlay::header(const char* caption)
    {
        return ImGui::CollapsingHeader(caption, ImGuiTreeNodeFlags_DefaultOpen);
    }

    bool UIOverlay::checkBox(const char* caption, bool* value)
    {
        bool res = ImGui::Checkbox(caption, value);
        if (res) { updated = true; };
        return res;
    }

    bool UIOverlay::checkBox(const char* caption, int32_t* value)
    {
        bool val = (*value == 1);
        bool res = ImGui::Checkbox(caption, &val);
        *value = val;
        if (res) { updated = true; };
        return res;
    }

    bool UIOverlay::inputFloat(const char* caption, float* value, float step, uint32_t precision)
    {
        bool res = ImGui::InputFloat(caption, value, step, step * 10.0f, precision);
        if (res) { updated = true; };
        return res;
    }

    bool UIOverlay::sliderFloat(const char* caption, float* value, float min, float max)
    {
        bool res = ImGui::SliderFloat(caption, value, min, max);
        if (res) { updated = true; };
        return res;
    }

    bool UIOverlay::sliderInt(const char* caption, int32_t* value, int32_t min, int32_t max)
    {
        bool res = ImGui::SliderInt(caption, value, min, max);
        if (res) { updated = true; };
        return res;
    }

    bool UIOverlay::comboBox(const char* caption, int32_t* itemindex, std::vector<std::string> items)
    {
        if (items.empty()) {
            return false;
        }
        std::vector<const char*> charitems;
        charitems.reserve(items.size());
        for (size_t i = 0; i < items.size(); i++) {
            charitems.push_back(items[i].c_str());
        }
        uint32_t itemCount = static_cast<uint32_t>(charitems.size());
        bool res = ImGui::Combo(caption, itemindex, &charitems[0], itemCount, itemCount);
        if (res) { updated = true; };
        return res;
    }

    bool UIOverlay::button(const char* caption)
    {
        bool res = ImGui::Button(caption);
        if (res) { updated = true; };
        return res;
    }

    void UIOverlay::text(const char* formatstr, ...)
    {
        va_list args;
        va_start(args, formatstr);
        ImGui::TextV(formatstr, args);
        va_end(args);
    }
}