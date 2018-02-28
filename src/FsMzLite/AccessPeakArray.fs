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
    
    /// Creates Peak1DArray of mzData array and intensityData Array
    let createPeak1DArray compression mzBinaryDataType intensityBinaryDataType (mzData:float []) (intensityData:float []) =
        match compression with
        | true -> 
            let peak1DArray = new Peak1DArray(BinaryDataCompressionType.ZLib,intensityBinaryDataType, mzBinaryDataType)
            let zipedData = Array.map2 (fun mz intz -> MzLite.Binary.Peak1D(intz,mz)) mzData intensityData 
            let newPeakA = MzLite.Commons.Arrays.MzLiteArray.ToMzLiteArray zipedData
            peak1DArray.Peaks <- newPeakA
            peak1DArray
        | false -> 
            let peak1DArray = new Peak1DArray(BinaryDataCompressionType.NoCompression,intensityBinaryDataType, mzBinaryDataType)
            let zipedData = Array.map2 (fun mz intz -> MzLite.Binary.Peak1D(intz,mz)) mzData intensityData 
            let newPeakA = MzLite.Commons.Arrays.MzLiteArray.ToMzLiteArray zipedData
            peak1DArray.Peaks <- newPeakA
            peak1DArray
