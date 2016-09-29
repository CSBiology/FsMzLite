namespace FsMzLite

module MassQuery = 

    open System
    open System.Collections.Generic;
    open System.Globalization;
    open System.IO

    open MzLite
    open MzLite.Binary
    open MzLite.Commons
    open MzLite.IO
    open MzLite.Json
    open MzLite.MetaData
    open MzLite.Model
    open MzLite.Processing
    open MzLite.SQL
    open MzLite.Wiff

    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization

    open Indexer   

    
    let createRangeQuery v offset =
        new RangeQuery(v, offset)

    let createRangeIndexQuery rt rtOffset mz mzOffset =
        new RtIndexerQuery(new RangeQuery(rt, rtOffset), new RangeQuery(mz, mzOffset))

    let createRangeIndexSwathQuery targetMz rt rtOffset ms2Masses =
        new SwathQuery(targetMz, new RangeQuery(rt, rtOffset), ms2Masses)

