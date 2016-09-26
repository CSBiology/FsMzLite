namespace FsMzLite

module AccessMassSpectrum = 

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
    

    /// accesses the Header of the WiffFile referenced by the path
    let getMassSpectraBy (reader:IMzLiteDataReader) runID = 
        reader.ReadMassSpectra(runID)
    
    /// Returns the ID of the MassSpectrum
    let getID (massSpectrum: MassSpectrum) =
        massSpectrum.ID  

    /// Returns a id-indexed massSpectrum
    let createIDIdxedMS (massSpectrum: MassSpectrum) =
        createIndexItemBy (getID massSpectrum) massSpectrum    
        
    /// Returns the MsLevel of the MassSpectrum 
    let getMsLevel (massSpectrum: MassSpectrum) = 
        if massSpectrum.CvParams.Contains("MS:1000511") then 
            (massSpectrum.CvParams.["MS:1000511"].Value) |> Convert.ToInt32
        else 
            -1

    /// Returns a msLevel-indexed massSpectrum
    let createMsLevelIdxedMS (massSpectrum: MassSpectrum) =
        createIndexItemBy (getMsLevel massSpectrum) massSpectrum   

    /// Returns the ScanTime (formerly: RetentionTime) of the MassSpectrum
    let getScanTime (massSpectrum: MassSpectrum) =  
        if massSpectrum.Scans.[0].CvParams.Contains("MS:1000016") then
            massSpectrum.Scans.[0].CvParams.["MS:1000016"].Value |> Convert.ToDouble        
        else 
            -1.    
    
    /// Returns a msLevel-indexed massSpectrum
    let createScanTimeIdxedMS (massSpectrum: MassSpectrum) =
        createIndexItemBy (getScanTime massSpectrum) massSpectrum  

    /// Returns PrecursorMZ of MS2 spectrum
    let getPrecursorMZ (massSpectrum: MassSpectrum) =
        if massSpectrum.Precursors.[0].SelectedIons.[0].CvParams.Contains("MS:1002234") then
            massSpectrum.Precursors.[0].SelectedIons.[0].CvParams.["MS:1002234"].Value:?> float  // |> Convert.ToInt32        
        else 
            -1.  

    /// Returns a precursorMZ-indexed massSpectrum
    let createPrecursorMZIdxedMS (massSpectrum: MassSpectrum) =
        createIndexItemBy (getScanTime massSpectrum) massSpectrum 
    
    /// Returns Range between two Features of two MassSpectra.  
    let initCreateRange (getter: MassSpectrum -> 'b) (ms: MassSpectrum) (consMS: MassSpectrum) =
        getter ms, getter consMS
    
    /// Returns Range theoretical Range between a real feature of the last MassSpectra and a type dependend infinityValue
    let initCreateLastRange (getter: MassSpectrum -> 'b) (infinityVal: 'b) (lastMS: MassSpectrum) =
        getter lastMS, infinityVal
    
    /// Returns function which can be used to determine the range between the scanTime of two MassSpectra. 
    let createScanTimeRange =
        initCreateRange getScanTime












 