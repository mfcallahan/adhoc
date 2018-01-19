## RestDump

#### A C# console app to dump all features from an ArcGIS Online feature service, regardless of the server's maximum record query limit.  

How it works:

ArcGIS Online allows a client to query a feature service and get a list of all object IDs, even if there are more than 1000 records (or whatever the server limit has been set to).  [Esri states](http://resources.arcgis.com/en/help/sds/rest/query.html): "Clients can exploit this to get all the query conforming object IDs by specifying returnIdsOnly=true and subsequently requesting feature sets for subsets of object IDs." This application will take this list of object IDs and spin up a maximum of 20 concurrent threads, each querying the feature service 1000 records at a time. The resulting output data is then stitched back together and dumped into a .csv file.