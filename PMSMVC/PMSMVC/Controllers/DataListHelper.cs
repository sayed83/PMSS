using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Text;

namespace PMSMVC.Controllers
{
    public static class DataListHelper
    {
        public static MvcHtmlString DataListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            var listId = ExpressionHelper.GetExpressionText(expression) + "_dataList";

            if (htmlAttributes == null)
                htmlAttributes = new object();

            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            dictionary.Add("list", listId);

            var input = html.TextBoxFor(expression, dictionary);
            var dataList = new TagBuilder("DataList");
            dataList.GenerateId(listId);

            StringBuilder items = new StringBuilder();
            foreach (var item in selectList)
            {
                items.AppendLine(ItemToOption(item));
            }

            dataList.InnerHtml = items.ToString();

            return new MvcHtmlString(input + dataList.ToString());
        }

        private static string ItemToOption(SelectListItem item)
        {
            TagBuilder builder = new TagBuilder("option");
            builder.MergeAttribute("value", item.Value);
            builder.SetInnerText(item.Text);

            return builder.ToString(TagRenderMode.Normal);
        }
    }
}