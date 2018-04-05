using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class InputExtensions
    {
        #region CheckBoxList
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">CheckBoxListInfo.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, List<CheckBoxListInfo> listInfo)
        {
            return htmlHelper.CheckBoxList
            (
                name,
                listInfo,
                (IDictionary<string, object>)null,
                0
            );
        }
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">CheckBoxListInfo.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, List<CheckBoxListInfo> listInfo, object htmlAttributes)
        {
            return htmlHelper.CheckBoxList
            (
                name,
                listInfo,
                (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes),
                0
            );
        }
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, List<CheckBoxListInfo> listInfo, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必须给CheckBoxList一个Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("必须要设置List<CheckBoxListInfo> listInfo");
            }
            if (listInfo.Count < 1)
            {
                throw new ArgumentException("List<CheckBoxListInfo> listInfo 至少要一组资料", "listInfo");
            }
            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;
            foreach (CheckBoxListInfo info in listInfo)
            {
                lineNumber++;
                TagBuilder builder = new TagBuilder("input");
                if (info.IsChecked)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                builder.InnerHtml = string.Format(" {0} ", info.DisplayText);
                sb.Append(builder.ToString(TagRenderMode.Normal));
                sb.Append("</br>");
            }
            return new MvcHtmlString(sb.ToString());
        }
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="number">每行的显示个数.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, List<CheckBoxListInfo> listInfo, IDictionary<string, object> htmlAttributes, int number)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必须给这些CheckBoxList一个Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("必须要设置List<CheckBoxListInfo> listInfo");
            }
            if (listInfo.Count < 1)
            {
                throw new ArgumentException("List<CheckBoxListInfo> listInfo 至少要有一组资料", "listInfo");
            }
            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;
            foreach (CheckBoxListInfo info in listInfo)
            {
                lineNumber++;
                TagBuilder builder = new TagBuilder("input");
                if (info.IsChecked)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                builder.InnerHtml = string.Format(" {0} ", info.DisplayText);
                sb.Append(builder.ToString(TagRenderMode.Normal));
                if (number == 0)
                {
                    sb.Append("<br />");
                }
                else if (lineNumber % number == 0)
                {
                    sb.Append("<br />");
                }
            }
            return new MvcHtmlString(sb.ToString());
        }
        #endregion
    }
}
