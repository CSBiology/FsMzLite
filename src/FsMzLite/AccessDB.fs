namespace FsMzLite


module AccessDB = 

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

    /// Create a new file instance of the DB schema. DELETES already existing instance
    let initDB filePath =
        let _ = System.IO.File.Delete filePath  
        let db = new MzLite.SQL.MzLiteSQL(filePath)
        db

    /// Returns the conncetion string to a existing MzLiteSQL DB
    let getConnection filePath =
        match System.IO.File.Exists filePath with
        | true  -> let db = new MzLite.SQL.MzLiteSQL(filePath)
                   db 
        | false -> initDB filePath

    /// Inserts MassSpectrum into DB schema
    let insertMSSpectrum (db: MzLiteSQL) runID (reader:IMzLiteDataReader) (spectrum: MassSpectrum) = 
        let peakArray = reader.ReadSpectrumPeaks(spectrum.ID)
        db.Insert(runID, spectrum, peakArray)
          
    /// Starts bulkinsert of mass spectra into a MzLiteSQL database
    let insertMSSpectraBy filepath runID (reader:IMzLiteDataReader) (spectra: seq<MassSpectrum>) = 
        let db = getConnection filepath
        let bulkInsert spectra = 
            spectra
            |> Seq.iter (insertMSSpectrum db runID reader)                                 
        let trans = db.BeginTransaction()
        bulkInsert spectra
        trans.Commit()
        trans.Dispose() 
        db.Dispose()
