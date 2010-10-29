﻿namespace CustomFilters
{
    using System;
    using Caliburn.PresentationFramework.Filters;
    using Caliburn.PresentationFramework.RoutedMessaging;

    //Note: A custom filter that will be executed before the action.
    //Note: Custom filters can also be IPostProcessor or IRescue
    public class UserInRoleAttribute : Attribute, IPreProcessor
    {
        private readonly string _role;

        public UserInRoleAttribute(string role)
        {
            _role = role;
        }

        public int Priority { get; set; }

        public bool AffectsTriggers
        {
            get { return true; }
        }

        //Note:  The code that will execute before the action.
        public bool Execute(IRoutedMessage message, IInteractionNode handlingNode, object[] parameters)
        {
            return CurrentUser.IsInRole(_role);
        }
    }
}