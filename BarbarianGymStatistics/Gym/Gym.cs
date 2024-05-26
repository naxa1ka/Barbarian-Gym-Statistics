using RestSharp;

namespace BarbarianGymStatistics;

public class Gym : IGym
{
    private readonly IRestClient _client;

    public Gym(IRestClient client)
    {
        _client = client;
    }

    public async Task<GymAvailability> GetGymAvailabilityAsync(
        CancellationToken cancellationToken = default
    )
    {
        const string body =
            @"<?xml version=""1.0"" encoding=""UTF-8""?><v:Envelope xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:d=""http://www.w3.org/2001/XMLSchema"" xmlns:c=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:v=""http://schemas.xmlsoap.org/soap/envelope/""><v:Header /><v:Body><n0:SQLExecWS xmlns:n0=""urn:WDSoapDB""><query><Connection><Source>CompassGym</Source><User></User><Password></Password><DatabaseName>compassgymdata</DatabaseName><DatabaseType></DatabaseType><OLEDBProvider>MSDASQL</OLEDBProvider><OptionalInformation></OptionalInformation></Connection><sql>SELECT doortbl.roomname, COUNT(memberintemp.PatientID) AS expr1, MAX(doortbl.doornotes) AS expr2, IF(app_companyinfotbl.intcol2 &gt; 0, app_companyinfotbl.intcol2 - ROUND(TIME_TO_SEC(TIMEDIFF(NOW(), MIN(memberintemp.DateIn))) / 60, 0), - 1) AS timein FROM app_companyinfotbl, doortbl INNER JOIN memberintemp ON doortbl.doornumber = memberintemp.incominglocation GROUP BY doortbl.roomname;</sql></query></n0:SQLExecWS></v:Body></v:Envelope>";
        var request = new RestRequest("//WDSOAPDB_WEB/WDSoapDB.rawws", Method.Post);
        request.AddHeader("Connection", "close");
        request.AddHeader("Accept-Encoding", "gzip");
        request.AddHeader("SOAPAction", "urn:WDSoapDB/SQLExecWS");
        request.AddHeader("Content-Type", "text/xml;charset=utf-8");
        request.AddBody(body, ContentType.Xml);
        var response = await _client.ExecuteAsync(request, cancellationToken);
        if (response.Content == null)
            throw new ArgumentNullException(nameof(response.Content));
        return GymAvailability.FromXml(response.Content);
    }
}
