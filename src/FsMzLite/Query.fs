namespace FsMzLite

module Query = 

    open MzLite.Processing
    open MzLite.Wiff
    open MzLite.IO
    
    ///
    let createRangeQuery v offset =
        new RangeQuery(v, offset)
    
    ///
    let getMS1RTIdx (reader:IMzLiteDataReader) runId = 
        reader.BuildRtIndex(runId)

    /// 
    let getXIC (reader:IMzLiteDataReader) (rtIdx:MzLite.Commons.Arrays.IMzLiteArray<MzLiteLinq.RtIndexEntry>) (rtQuery:RangeQuery) (mzQuery:RangeQuery) = 
        reader.RtProfile(rtIdx, rtQuery, mzQuery) 

    ///
    let getXICs (reader:IMzLiteDataReader) (rtIdx:MzLite.Commons.Arrays.IMzLiteArray<MzLiteLinq.RtIndexEntry>) (rtQuery:RangeQuery) (mzQueries:RangeQuery []) = 
        reader.RtProfiles(rtIdx, rtQuery, mzQueries) 
       
    ///       
    let createSwathQuery targetMz rtQuery ms2MzQueries =
        new SwathQuery(targetMz, rtQuery, ms2MzQueries)

    ///
    let getSwathIdx (reader:IMzLiteDataReader) runId =
        SwathIndexer.Create(reader, runId)

    ///
    let getSwathXics (reader:IMzLiteDataReader) (swathIdx:SwathIndexer) swathQuery = 
        swathIdx.GetMS2(reader, swathQuery)

    ///        
    let getSwathXICsBy (reader:IMzLiteDataReader) (swathIdx:SwathIndexer) (rtQuery:RangeQuery) (ms2MzQueries:RangeQuery []) tarMz = 
        let swathQ = createSwathQuery tarMz rtQuery ms2MzQueries
        getSwathXics reader swathIdx swathQ
        
       
