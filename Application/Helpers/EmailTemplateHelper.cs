using System.IO;
using System.Threading.Tasks;

public static class EmailTemplateHelper
{
    public static async Task<string> GetTemplateAsync(string templatePath, Dictionary<string, string> placeholders)
    {
        var template = await File.ReadAllTextAsync(templatePath);
        foreach (var pair in placeholders)
        {
            template = template.Replace($"{{{{{pair.Key}}}}}", pair.Value);
        }
        return template;
    }
}