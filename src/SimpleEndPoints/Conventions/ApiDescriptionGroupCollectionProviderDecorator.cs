using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using SimpleEndpoints.Core;
using SimpleEndpoints.VerbScoped;

namespace SimpleEndpoints.Conventions
{
    internal class ApiDescriptionGroupCollectionProviderDecorator : IApiDescriptionGroupCollectionProvider
    {
        private readonly IApiDescriptionGroupCollectionProvider _inner;

        public ApiDescriptionGroupCollectionProviderDecorator(IApiDescriptionGroupCollectionProvider inner)
        {
            _inner = inner;
        }

        public ApiDescriptionGroupCollection ApiDescriptionGroups
        {
            get
            {
                foreach (var apiDescriptionGroup in _inner.ApiDescriptionGroups.Items)
                {
                    foreach (var apiDescription in apiDescriptionGroup.Items)
                    {
                        if (apiDescription.ActionDescriptor is ControllerActionDescriptor controller)
                        {
                            if (controller.ControllerTypeInfo.GetCustomAttributes(typeof(SimpleEndpointAttribute),
                                true).FirstOrDefault() is SimpleEndpointAttribute attribute)
                            {
                                apiDescription.HttpMethod = attribute.HttpVerb;
                            }
                        }
                    }
                }

                return _inner.ApiDescriptionGroups;
            }
        }
    }
}