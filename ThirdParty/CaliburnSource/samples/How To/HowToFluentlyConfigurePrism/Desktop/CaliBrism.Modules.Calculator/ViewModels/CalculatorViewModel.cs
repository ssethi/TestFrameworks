using System;
using System.Windows;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Filters;

namespace CaliBrism.Modules.Calculator.ViewModels
{
    using Caliburn.Core.IoC;
    using Caliburn.PresentationFramework.Screens;

    [Rescue("GeneralRescue")]
    [PerRequest(typeof(ICalculatorViewModel))]
    public class CalculatorViewModel : Screen, ICalculatorViewModel
    {
        public override string DisplayName
        {
            get
            {
                return "Calculator";
            }
            set
            {
                base.DisplayName = value;
            }
        }
       
        [Rescue("ActionSpecificRescue")]
        [Preview("CanDivide")]
        public int Divide(int left, int right)
        {
            return left / right;
        }

        public bool CanDivide(int left, int right)
        {
            return right != 0;
        }

        public void GeneralRescue(Exception ex)
        {
            //Note: This is for demo purposes only.
            //Note: It is not a good practice to call MessageBox.Show from a non-View class.
            //Note: Consider implementing a MessageBoxService.
            MessageBox.Show(ex.Message);
        }

        public void ActionSpecificRescue(Exception ex)
        {
            //Note: This is for demo purposes only.
            //Note: It is not a good practice to call MessageBox.Show from a non-View class.
            //Note: Consider implementing a MessageBoxService.
            MessageBox.Show("Divide Action: " + ex.Message);
        }
    }
}