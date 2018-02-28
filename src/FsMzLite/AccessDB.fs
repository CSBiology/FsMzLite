namespace FsMzLite


module AccessDB = 

    open System
    open System.IO

    open MzLite.Binary
    open MzLite.IO
    open MzLite.Model
    open MzLite.SQL

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

    /// copies MassSpectrum into DB schema
    let insertMSSpectrum (db: MzLiteSQL) runID (reader:IMzLiteDataReader) (compress: bool) (spectrum: MassSpectrum) = 
        let peakArray = reader.ReadSpectrumPeaks(spectrum.ID)
        match compress with 
        | true  -> 
            let clonedP = new Peak1DArray(BinaryDataCompressionType.ZLib,peakArray.IntensityDataType,peakArray.MzDataType)
            clonedP.Peaks <- peakArray.Peaks
            db.Insert(runID, spectrum, clonedP)
        | false -> 
            let clonedP = new Peak1DArray(BinaryDataCompressionType.NoCompression,peakArray.IntensityDataType,peakArray.MzDataType)
            clonedP.Peaks <- peakArray.Peaks
            db.Insert(runID, spectrum, clonedP)

    /// modifies spectrum according to the used spectrumPeaksModifierF and inserts the result into the DB schema 
    let insertModifiedSpectrumBy (spectrumPeaksModifierF: IMzLiteDataReader -> MassSpectrum -> bool -> Peak1DArray ) (db: MzLiteSQL) runID (reader:IMzLiteDataReader) (compress: bool) (spectrum: MassSpectrum) = 
        let modifiedP = 
            spectrumPeaksModifierF reader spectrum compress
        db.Insert(runID, spectrum, modifiedP)

    /// Starts bulkinsert of mass spectra into a MzLiteSQL database
    let insertMSSpectraBy insertSpectrumF outFilepath runID (reader:IMzLiteDataReader) (compress: bool) (spectra: seq<MassSpectrum>) = 
        let db = getConnection outFilepath
        let bulkInsert spectra = 
            spectra
            |> Seq.iter (insertSpectrumF db runID reader compress)                                 
        let trans = db.BeginTransaction()
        bulkInsert spectra
        trans.Commit()
        trans.Dispose() 
        db.Dispose()

    ///// copies MassSpectrum into DB schema
    //let insertMSSpectrumAsync (db: MzLiteSQL) runID (reader:IMzLiteDataReader) (compress: bool) (spectrum: MassSpectrum) = 
    //    let peakArray = reader.ReadSpectrumPeaks(spectrum.ID)
    //    match compress with 
    //    | true  -> 
    //        let clonedP = new Peak1DArray(BinaryDataCompressionType.ZLib,peakArray.IntensityDataType,peakArray.MzDataType)
    //        clonedP.Peaks <- peakArray.Peaks
    //        db.InsertAsync(runID, spectrum, clonedP)
    //    | false -> 
    //        let clonedP = new Peak1DArray(BinaryDataCompressionType.NoCompression,peakArray.IntensityDataType,peakArray.MzDataType)
    //        clonedP.Peaks <- peakArray.Peaks
    //        db.InsertAsync(runID, spectrum, clonedP)
    
    ///// modifies spectrum according to the used spectrumPeaksModifierF and inserts the result into the DB schema 
    //let insertModifiedSpectrumAsyncBy (spectrumPeaksModifierF: IMzLiteDataReader -> MassSpectrum -> bool -> Peak1DArray ) (db: MzLiteSQL) runID (reader:IMzLiteDataReader) (compress: bool) (spectrum: MassSpectrum) = 
    //    let modifiedP = 
    //        spectrumPeaksModifierF reader spectrum compress
    //    db.InsertAsync(runID, spectrum, modifiedP).RunSynchronously
                
    ///// Starts bulkinsert of mass spectra into a MzLiteSQL database
    //let insertMSSpectraAsyncBy insertSpectrumFAsync outFilepath runID (reader:IMzLiteDataReader) (compress: bool) (spectra: seq<MassSpectrum>) = 
    //    let db = getConnection outFilepath
    //    let bulkInsert spectra = 
    //        [for i in spectra -> async { return (insertSpectrumFAsync db runID reader compress i)}]
    //        |> Async.Parallel
    //        |> Async.RunSynchronously
    //    let trans = db.BeginTransaction()
    //    bulkInsert spectra
    //    trans.Commit()
    //    trans.Dispose() 
    //    db.Dispose()
