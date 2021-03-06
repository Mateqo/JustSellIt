#pragma checksum "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "93e2685602a4263d0e86c467048d80f1c37a9382"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ProductTile), @"mvc.1.0.view", @"/Views/Shared/_ProductTile.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\_ViewImports.cshtml"
using JustSellIt.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\_ViewImports.cshtml"
using JustSellIt.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"93e2685602a4263d0e86c467048d80f1c37a9382", @"/Views/Shared/_ProductTile.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8e3d01b7d06a054c7c33136c9cfabf905f146de7", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__ProductTile : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<JustSellIt.Application.Interfaces.IProductList>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Product", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ProductDetails", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n<div class=\"row\">\r\n");
#nullable restore
#line 5 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
      foreach (var product in Model.Products)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"col-sm-6  col-md-4 col-lg-3 ml-auto mr-auto mt-4\">\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "93e2685602a4263d0e86c467048d80f1c37a93824219", async() => {
                WriteLiteral("\r\n                    <div class=\"tile\">\r\n");
#nullable restore
#line 10 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                          
                            if (product.Status != "Published")
                            {
                                switch (product.Status)
                                {
                                    case "Rejected":

#line default
#line hidden
#nullable disable
                WriteLiteral("                                        <div class=\"info-tile-rejected\">Odrzucone</div>\r\n");
#nullable restore
#line 17 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                                        break;
                                    case "ForVeryfication":

#line default
#line hidden
#nullable disable
                WriteLiteral("                                        <div class=\"info-tile-veryfication\">W trakcie weryfikacji</div>\r\n");
#nullable restore
#line 20 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                                        break;
                                    case "Draft":

#line default
#line hidden
#nullable disable
                WriteLiteral("                                        <div class=\"info-tile-draft\">Wersja robocza</div>\r\n");
#nullable restore
#line 23 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                                        break;
                                }
                            }
                        

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <img src=\"/images/other/none.png\" alt=\"Image product\" class=\"img-fluid mb-3\" />\r\n                        <div class=\"title\">\r\n                            <h3>");
#nullable restore
#line 29 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                           Write(product.Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("</h3>\r\n                        </div>\r\n                        <span>");
#nullable restore
#line 31 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                         Write(product.Location);

#line default
#line hidden
#nullable disable
                WriteLiteral("</span><span>");
#nullable restore
#line 31 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                                                       Write(product.CreatedOn.ToString("dd/MM/yyyy"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</span>\r\n                        <h4>");
#nullable restore
#line 32 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                       Write(product.Price);

#line default
#line hidden
#nullable disable
                WriteLiteral(" zł</h4>\r\n                        <i class=\"far fa-heart\"></i>\r\n                    </div>\r\n                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 8 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
                                                                          WriteLiteral(product.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </div>\r\n");
#nullable restore
#line 37 "D:\Users\User\Desktop\repozytorium\JustSellIt\JustSellIt\JustSellIt.Web\Views\Shared\_ProductTile.cshtml"
        }
    

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<JustSellIt.Application.Interfaces.IProductList> Html { get; private set; }
    }
}
#pragma warning restore 1591
