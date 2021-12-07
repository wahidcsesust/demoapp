using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace HealthCare.Web.Extensions
{
    public static class HtmlPrefixScopeExtensions
    {
        private const string IDS_TO_REUSE_KEY = "__htmlPrefixScopeExtensions_IdsToReuse_";

        /// <summary>
        /// Begins the collection item.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns></returns>
        public static IDisposable BeginCollectionItem(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper html, string collectionName)
        {
            var idsToReuse = GetIdsToReuse(html.ViewContext.HttpContext, collectionName);
            string itemIndex = idsToReuse.Count > 0 ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();

            return BeginCollectionItem(html, collectionName, itemIndex);
        }

        /// <summary>
        /// Begins the collection item.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns></returns>
        public static IDisposable BeginCollectionItem(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper html, string collectionName, string itemIndex)
        {
            if (!string.IsNullOrWhiteSpace(collectionName) && !string.IsNullOrWhiteSpace(itemIndex))
            {
                // autocomplete="off" is needed to work around a very annoying Chrome behaviour whereby it reuses old values after the user clicks "Back", which causes the xyz.index and xyz[...] values to get out of sync.
                html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" id=\"{0}_Index\" name=\"{0}.Index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(itemIndex)));

                return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, string.Format("{0}[{1}]", collectionName, itemIndex));
            }

            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, null);
        }

        private static Queue<string> GetIdsToReuse(HttpContext httpContext, string collectionName)
        {
            // We need to use the same sequence of IDs following a server-side validation failure,  
            // otherwise the framework won't render the validation error messages next to each item.

            string key = IDS_TO_REUSE_KEY + collectionName;
            var queue = (Queue<string>)httpContext.Items[key];
            if (queue == null)
            {
                httpContext.Items[key] = queue = new Queue<string>();
                var previouslyUsedIds = httpContext.Request.ToString();//[collectionName + ".Index"];
                if (!string.IsNullOrEmpty(previouslyUsedIds))
                    foreach (string previouslyUsedId in previouslyUsedIds.Split(','))
                        queue.Enqueue(previouslyUsedId);
            }
            return queue;
        }

        #region Nested type: HtmlFieldPrefixScope

        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo _templateInfo;
            private readonly string _previousHtmlFieldPrefix;

            /// <summary>
            /// Initializes a new instance of the <see cref="HtmlFieldPrefixScope"/> class.
            /// </summary>
            /// <param name="templateInfo">The template info.</param>
            /// <param name="htmlFieldPrefix">The HTML field prefix.</param>
            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                _templateInfo = templateInfo;
                _previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;

                if (!string.IsNullOrWhiteSpace(htmlFieldPrefix))
                    templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            #region IDisposable Members

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousHtmlFieldPrefix;
            }

            #endregion
        }

        #endregion

        //private const string IDS_TO_REUSE_KEY = "__htmlPrefixScopeExtensions_IdsToReuse_";

        ///// <summary>
        ///// Begins the collection item.
        ///// </summary>
        ///// <param name="html">The HTML.</param>
        ///// <param name="collectionName">Name of the collection.</param>
        ///// <returns></returns>
        //public static IDisposable BeginCollectionItem(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper html, string collectionName)
        //{
        //    var idsToReuse = GetIdsToReuse(html.ViewContext.HttpContext, collectionName);
        //    string itemIndex = idsToReuse.Count > 0 ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();

        //    return BeginCollectionItem(html, collectionName, itemIndex);
        //}

        ///// <summary>
        ///// Begins the collection item.
        ///// </summary>
        ///// <param name="html">The HTML.</param>
        ///// <param name="collectionName">Name of the collection.</param>
        ///// <param name="itemIndex">Index of the item.</param>
        ///// <returns></returns>
        //public static IDisposable BeginCollectionItem(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper html, string collectionName, string itemIndex)
        //{
        //    if (!string.IsNullOrWhiteSpace(collectionName) && !string.IsNullOrWhiteSpace(itemIndex))
        //    {
        //        // autocomplete="off" is needed to work around a very annoying Chrome behaviour whereby it reuses old values after the user clicks "Back", which causes the xyz.index and xyz[...] values to get out of sync.
        //        html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" id=\"{0}_Index\" name=\"{0}.Index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(itemIndex)));

        //        return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, string.Format("{0}[{1}]", collectionName, itemIndex));
        //    }

        //    return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, null);
        //}

        //private static Queue<string> GetIdsToReuse(HttpContext httpContext, string collectionName)
        //{
        //    // We need to use the same sequence of IDs following a server-side validation failure,  
        //    // otherwise the framework won't render the validation error messages next to each item.
        //    string key = IDS_TO_REUSE_KEY + collectionName;
        //    var queue = (Queue<string>)httpContext.Items[key];
        //    if (queue == null)
        //    {
        //        httpContext.Items[key] = queue = new Queue<string>();
        //        var previouslyUsedIds = "";
        //        if (!string.IsNullOrEmpty(previouslyUsedIds))
        //            foreach (string previouslyUsedId in previouslyUsedIds.Split(','))
        //                queue.Enqueue(previouslyUsedId);
        //    }
        //    return queue;
        //}

        //#region Nested type: HtmlFieldPrefixScope

        //private class HtmlFieldPrefixScope : IDisposable
        //{
        //    private readonly TemplateInfo _templateInfo;
        //    private readonly string _previousHtmlFieldPrefix;

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="HtmlFieldPrefixScope"/> class.
        //    /// </summary>
        //    /// <param name="templateInfo">The template info.</param>
        //    /// <param name="htmlFieldPrefix">The HTML field prefix.</param>
        //    public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
        //    {
        //        _templateInfo = templateInfo;
        //        _previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;

        //        if (!string.IsNullOrWhiteSpace(htmlFieldPrefix))
        //            templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
        //    }

        //    #region IDisposable Members

        //    public void Dispose()
        //    {
        //        _templateInfo.HtmlFieldPrefix = _previousHtmlFieldPrefix;
        //    }

        //    #endregion
        //}

        //#endregion
    }
}
