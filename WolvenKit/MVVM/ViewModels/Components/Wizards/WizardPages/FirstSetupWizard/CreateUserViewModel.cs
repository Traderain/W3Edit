﻿using Catel;
using Catel.Data;
using Catel.Fody;
using Catel.IoC;
using Catel.MVVM;
using WolvenKit.Model.Wizards;

namespace WolvenKit.ViewModels.Wizards.WizardPages.FirstSetupWizard
{
    class CreateUserViewModel : ViewModelBase
    {
        #region constructors
        public CreateUserViewModel(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            FirstSetupWizardModel = serviceLocator.ResolveType<FirstSetupWizardModel>();
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the FirstSetupWizardModel.
        /// </summary>
        [Model]
        [Expose("Author")]
        [Expose("Email")]
        [Expose("DonateLink")]
        [Expose("DefaultDescription")]
        public FirstSetupWizardModel FirstSetupWizardModel
        {
            get { return GetValue<FirstSetupWizardModel>(ProjectWizardModelProperty); }
            set { SetValue(ProjectWizardModelProperty, value); }
        }

        /// <summary>
        /// Register the FirstSetupWizardModel property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ProjectWizardModelProperty = RegisterProperty("FirstSetupWizardModel", typeof(FirstSetupWizardModel));
        #endregion properties
    }
}