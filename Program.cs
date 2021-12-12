// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using System.Text.Json;
using System.IO;
using ConsoleApp1.MODELS;

HttpClient http_client = new HttpClient();

string staging_har = await File.ReadAllTextAsync("brasasclub-staging.azurewebsites.net.har");
string prod_har = await File.ReadAllTextAsync("brasasclub.com.har");

var staging_page = JsonSerializer.Deserialize<ROOT>(staging_har);
var prod_page = JsonSerializer.Deserialize<ROOT>(prod_har);

if(staging_page.log.entries.Count() != prod_page.log.entries.Count())
{
    Console.WriteLine("NOT EQUAL NUM ENTRIES");
}
else
{
    Console.WriteLine("EQUAL NUM ENTRIES");
}

var staging_entries_scripts = staging_page.log.entries.Where(x => x._resourceType == "script" || x._resourceType == "fetch" || x._resourceType == "xhr");
var prod_entries_scripts = prod_page.log.entries.Where(x => x._resourceType == "script" || x._resourceType == "fetch" || x._resourceType == "xhr");

var matching_entries = new Dictionary<ENTRY, ENTRY>();

foreach (var prod_entry in prod_entries_scripts)
{
    string prod_request_url = prod_entry.request.url.Replace("brasasclub.com", "");
    ENTRY staging_entry;
    try
    {
        staging_entry = staging_entries_scripts.Single(x => x.request.url.Replace("brasasclub-staging.azurewebsites.net", "") == prod_request_url);
    }
    catch
    {
        Console.WriteLine($"UNMATCHED ENTRY ({prod_request_url})");
        continue;
    }
    string staging_request_url = staging_entry.request.url.Replace("brasasclub-staging.azurewebsites.net", "");

    if(staging_request_url == prod_request_url)
    {
        matching_entries.Add(staging_entry, prod_entry);
    }
}

Console.WriteLine($"ALL ENTRIES ({matching_entries.Count()} / {prod_entries_scripts.Count()}) MATCHED");

foreach(var match in matching_entries)
{
    if(match.Key.response.content.text != match.Value.response.content.text)
    {
        Console.WriteLine($"UNEQUAL MATCHES: ({match.Key.request.url}) ({match.Value.request.url})");
        return;
    }
}

Console.WriteLine("ALL MATCHED ENTRIES ARE EQUAL");