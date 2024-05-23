namespace BarbarianGymStatistics.Tests;

public static class GymAvailabilityTestDataFactory
{
    public static string CreateXmlResponse(int liveCount, int capacity)
    {
        return $"""
                           <?xml version="1.0" encoding="UTF-8"?>
                                       <SOAP-ENV:Envelope xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/">
                                           <SOAP-ENV:Header/>
                                           <SOAP-ENV:Body>
                                               <ns1:SQLExecWSResponse xmlns:ns1="urn:WDSoapDB">
                                                   <SQLExecWSResult>
                                                       <Error>
                                                           <ErrorCode/>
                                                           <ErrorMsg/>
                                                       </Error>
                                                       <Description>
                                                           roomname    T   -1  255&#13;
                                                           expr1   N   3   20&#13;
                                                           expr2   T   -1  45&#13;
                                                           timein  N   11  12
                                                       </Description>
                                                       <DataSet>
                                                           <n>1</n>
                                                           <r>
                                                               <c>Gym</c>
                                                               <c>{liveCount}</c>
                                                               <c>{capacity}</c>
                                                               <c>-1</c>
                                                           </r>
                                                       </DataSet>
                                                   </SQLExecWSResult>
                                               </ns1:SQLExecWSResponse>
                                           </SOAP-ENV:Body>
                                       </SOAP-ENV:Envelope>
                           """;
    }
}
