using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NATS.Helpers;

[HtmlTargetElement("input", Attributes = ForAttributeName)]
[HtmlTargetElement("select", Attributes = ForAttributeName)]
[HtmlTargetElement("textarea", Attributes = ForAttributeName)]
[UsedImplicitly]
public class BootstrapFormControlTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-for";
    
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For { get; set; }
    
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        List<string> classList = new List<string>
        {
            output.Attributes["class"]?.Value.ToString(),
            ComputeClassNameBasedOnTagName(context),
            ComputeClassNameBasedOnValidationState()
        };

        List<string> filteredClassList = classList
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToList();

        if (filteredClassList.Count > 0)
        {
            output.Attributes.SetAttribute("class", string.Join(" ", filteredClassList));
        }
    }

    private string ComputeClassNameBasedOnValidationState()
    {
        if (ViewContext.ViewData.ModelState.TryGetValue(For.Name, out var entry))
        {
            bool isFieldValidated;
            isFieldValidated = entry.ValidationState != ModelValidationState.Unvalidated;
            bool isFieldInvalid = entry.Errors.Any();

            if (!isFieldValidated)
            {
                return string.Empty;
            }

            return isFieldInvalid ? "is-invalid" : "is-valid";
        }

        return string.Empty;
    }

    private static string ComputeClassNameBasedOnTagName(TagHelperContext context)
    {
        string tagName = context.TagName.ToLower();
        switch (tagName)
        {
            case "select":
                return "form-select";
            case "input" or "textarea":
                return "form-control";
            default:
                return string.Empty;
        }
    }
}