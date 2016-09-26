namespace FsMzLite

module AccessPeakArray = 
    
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

    /// Returns a MzLite.Binary.Peak1DArray
    let getPeak1DArray (reader:IMzLiteDataReader) msID = 
        reader.ReadSpectrumPeaks(msID)

    /// Returns a mzData Array of a peak1DArray
    let mzDataOf (peak1DArray: MzLite.Binary.Peak1DArray) =
        peak1DArray.Peaks 
        |> Seq.map (fun peak -> peak.Mz)
        |> Array.ofSeq

    /// Returns a intensityData Array of a peak1DArray
    let intensityDataOf (peak1DArray: MzLite.Binary.Peak1DArray) =
        peak1DArray.Peaks 
        |> Seq.map (fun peak -> peak.Intensity)
        |> Array.ofSeq
        
    /// Returns tuple of a mzData Array and intensityData Array of a peak1DArray
    let mzIntensityArrayOf (peak1DArray: MzLite.Binary.Peak1DArray) =
         peak1DArray.Peaks
         |> Seq.map (fun peak -> peak.Mz, peak.Intensity) //TODO  mutable Ansatz
         |> Array.ofSeq
         |> Array.unzip
