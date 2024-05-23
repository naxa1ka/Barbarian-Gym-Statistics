using System.Text;

namespace BarbarianGymStatistics;

public class GymAvailabilityService : IGymAvailabilityService
{
    private readonly IHttpClient _httpClient;

    public GymAvailabilityService(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GymAvailability> GetGymAvailabilityAsync()
    {
        const string url = "http://barbarian.myftp.org:1515//WDSOAPDB_WEB/WDSoapDB.rawws";
        const string body = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><v:Envelope xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:d=\"http://www.w3.org/2001/XMLSchema\" xmlns:c=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:v=\"http://schemas.xmlsoap.org/soap/envelope/\"><v:Header /><v:Body><n0:SQLExecWS xmlns:n0=\"urn:WDSoapDB\"><query><Connection><Source>CompassGym</Source><User></User><Password></Password><DatabaseName>compassgymdata</DatabaseName><DatabaseType></DatabaseType><OLEDBProvider>MSDASQL</OLEDBProvider><OptionalInformation></OptionalInformation></Connection><sql>SELECT doortbl.roomname, COUNT(memberintemp.PatientID) AS expr1, MAX(doortbl.doornotes) AS expr2, IF(app_companyinfotbl.intcol2 &gt; 0, app_companyinfotbl.intcol2 - ROUND(TIME_TO_SEC(TIMEDIFF(NOW(), MIN(memberintemp.DateIn))) / 60, 0), - 1) AS timein FROM app_companyinfotbl, doortbl INNER JOIN memberintemp ON doortbl.doornumber = memberintemp.incominglocation GROUP BY doortbl.roomname;</sql></query></n0:SQLExecWS></v:Body></v:Envelope>";

        var content = new StringContent(body, Encoding.UTF8, "text/xml");
        content.Headers.Add("User-Agent", "My Fitness Trainer");
        content.Headers.Add("Connection", "close");
        content.Headers.Add("Accept-Encoding", "gzip");
        content.Headers.Add("SOAPAction", "urn:WDSoapDB/SQLExecWS");

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return GymAvailability.FromXml(responseContent);
    }
}