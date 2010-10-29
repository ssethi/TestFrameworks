//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using EmployeeModule.Models;
using UIComposition.EmployeeModule.Models;

namespace UIComposition.EmployeeModule.Services
{
    /// <summary>
    /// Dummy employee data service class. Provides dummy data for employees and projects.
    /// Replace with your real employee data access or employee data service proxy.
    /// </summary>
    public class EmployeeDataService : IEmployeeDataService
    {
        private Employees employees;
        private Projects projects;

        public Employees GetEmployees()
        {
            if (this.employees == null)
            {
                // Dummy Data.
                this.employees = new Employees
                                      {
                                          new Employee()
                                              {
                                                  Id = "1",
                                                  Name = "John",
                                                  LastName = "Smith",
                                                  Phone = "(425) 555 8912",
                                                  Email = "John.Smith@Contoso.com"
                                              },
                                          new Employee()
                                              {
                                                  Id = "2",
                                                  Name = "Bonnie",
                                                  LastName = "Skelly",
                                                  Phone = "(206) 555 7301",
                                                  Email = "Bonnie.Skelly@Contoso.com"
                                              },
                                          new Employee()
                                              {
                                                  Id = "3",
                                                  Name = "Lisa",
                                                  LastName = "Blunt",
                                                  Phone = "(425) 555 7492",
                                                  Email = "Lisa.Blunt@Contoso.com"
                                              },
                                          new Employee()
                                              {
                                                  Id = "4",
                                                  Name = "Kylie",
                                                  LastName = "Pugh",
                                                  Phone = "(425) 555 2836",
                                                  Email = "Kylie.Pugh@Contoso.com"
                                              },
                                      };
            }

            return this.employees;
        }

        public Projects GetProjects()
        {
            if (this.projects == null)
            {
                // Dummy data.
                this.projects = new Projects
                                     {
                                         new Project() {Id = "1", ProjectName = "Project 1", Role = "Dev Lead"},
                                         new Project() {Id = "1", ProjectName = "Project 2", Role = "Tech Reviewer"},
                                         new Project() {Id = "2", ProjectName = "Project 1", Role = "Test Lead"},
                                         new Project() {Id = "2", ProjectName = "Project 2", Role = "Tech Reviewer"},
                                         new Project() {Id = "3", ProjectName = "Project 1", Role = "Architect"},
                                         new Project() {Id = "3", ProjectName = "Project 2", Role = "Tech Reviewer"},
                                         new Project() {Id = "3", ProjectName = "Project 3", Role = "Tech Reviewer"},
                                         new Project() {Id = "4", ProjectName = "Project 1", Role = "Test Lead"},
                                         new Project() {Id = "4", ProjectName = "Project 2", Role = "Tech Reviewer"},
                                         new Project() {Id = "4", ProjectName = "Project 3", Role = "Tech Reviewer"},
                                         new Project() {Id = "4", ProjectName = "Project 4", Role = "Tech Reviewer"}
                                     };
            }

            return this.projects;
        }
    }
}