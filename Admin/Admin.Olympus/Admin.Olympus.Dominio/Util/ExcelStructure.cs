using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Xml.Linq;


/// <summary>
/// Summary description for ExcelStructure
/// </summary>
public class ExcelStructure
{
    #region constants

    public const int TYPE_STRING = 0;
    public const int TYPE_SHARED_STRING = 1;
    public const int TYPE_INLINE_STRING = 2;
    public const int TYPE_NUMBER = 3;
    public const int TYPE_DATE = 4;
    public const int TYPE_BOOLEAN = 5;
    public const int TYPE_ERROR = 6;
    public const int TYPE_FORMULA = 10;

    #endregion
    #region properties
    SpreadsheetDocument _spreadsheetDocument;
    public SpreadsheetDocument spreadsheetDocument
    {
        get { return _spreadsheetDocument; }
        set { _spreadsheetDocument = value; }
    }

    FileVersion _fileVersion;
    public FileVersion fileVersion
    {
        get { return _fileVersion; }
        set { _fileVersion = value; }
    }

    Workbook _workbook;
    public Workbook workbook
    {
        get { return _workbook; }
        set { _workbook = value; }
    }

    WorkbookPart _workbookpart;
    public WorkbookPart workbookpart
    {
        get { return _workbookpart; }
        set { _workbookpart = value; }
    }

    WorkbookStylesPart _stylesPart;
    public WorkbookStylesPart stylesPart
    {
        get { return _stylesPart; }
        set { _stylesPart = value; }
    }

    Stylesheet _stylesSheet;
    public Stylesheet stylesSheet
    {
        get { return _stylesSheet; }
        set { _stylesSheet = value; }
    }

    WorksheetPart _worksheetPart;
    public WorksheetPart worksheetPart
    {
        get { return _worksheetPart; }
        set { _worksheetPart = value; }
    }

    Worksheet _worksheet;
    public Worksheet worksheet
    {
        get { return _worksheet; }
        set { _worksheet = value; }
    }

    Sheets _sheets;
    public Sheets sheets
    {
        get { return _sheets; }
        set { _sheets = value; }
    }

    Sheet _sheet;
    public Sheet sheet
    {
        get { return _sheet; }
        set { _sheet = value; }
    }

    Columns _columns;
    public Columns columns
    {
        get { return _columns; }
        set { _columns = value; }
    }

    SheetData _sheetData;
    public SheetData sheetData
    {
        get { return _sheetData; }
        set { _sheetData = value; }
    }

    DrawingsPart _drawingsPart;
    public DrawingsPart drawingsPart
    {
        get { return _drawingsPart; }
        set { _drawingsPart = value; }
    }

    WorksheetDrawing _worksheetDrawing;
    public WorksheetDrawing worksheetDrawing
    {
        get { return _worksheetDrawing; }
        set { _worksheetDrawing = value; }
    }

    ImagePart _imagePart;
    public ImagePart imagePart
    {
        get { return _imagePart; }
        set { _imagePart = value; }
    }


    #endregion

    //Create the structure in memory
    public ExcelStructure(ref MemoryStream stream, bool autoSave = true)
    {
        // Create a spreadsheet document by supplying the filepath.
        // By default, AutoSave = true, Editable = true, and Type = xlsx.
        this._spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, autoSave);

        this.generateFile();
    }

    //Create the structure in memory
    public ExcelStructure(string sheetName, ref MemoryStream stream, bool autoSave = true)
    {
        // Create a spreadsheet document by supplying the filepath.
        // By default, AutoSave = true, Editable = true, and Type = xlsx.
        this._spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, autoSave);

        this.generateFile(sheetName);
    }

    //Create the structure from a file path
    public ExcelStructure(string path, bool autoSave = true)
    {
        // Create a spreadsheet document by supplying the filepath.
        // By default, AutoSave = true, Editable = true, and Type = xlsx.
        this._spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook, autoSave);

        this.generateFile();
    }

    //Create the structure from a file path
    public ExcelStructure(string sheetName, string path, bool autoSave = true)
    {
        // Create a spreadsheet document by supplying the filepath.
        // By default, AutoSave = true, Editable = true, and Type = xlsx.
        this._spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook, autoSave);

        this.generateFile(sheetName);
    }

    private void generateFile(string sheetName = "mySheet")
    {
        // Add a WorkbookPart to the document.
        this._workbook = new Workbook();
        this._workbookpart = spreadsheetDocument.AddWorkbookPart();
        this._workbookpart.Workbook = this._workbook;

        this._fileVersion = new FileVersion();
        _fileVersion.ApplicationName = "Microsoft Office Excel";

        this._workbook.Append(this._fileVersion);

        // Add a WorksheetPart to the WorkbookPart.
        this._worksheet = new Worksheet();
        this._worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
        this._worksheetPart.Worksheet = this._worksheet;

        this._stylesPart = this._spreadsheetDocument.WorkbookPart.AddNewPart<WorkbookStylesPart>();
        this._stylesSheet = generateStyleSheet();
        this._stylesPart.Stylesheet = this._stylesSheet;

        stylesPart.Stylesheet.Save();

        // Add Sheets to the Workbook.
        this._sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

        // Append a new worksheet and associate it with the workbook.
        this._sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
        this._sheets.Append(sheet);

        this._columns = new Columns();
        this._worksheet.Append(this._columns);
    }

    // create first sheetData cell table.
    public void createSheetData()
    {
        this._sheetData = new SheetData();
        this._worksheet.Append(this._sheetData);//worksheetPart.Worksheet.GetFirstChild<SheetData>();
    }

    public void close()
    {
        this._spreadsheetDocument.Close();
    }

    //Diagram diagram, ImageSetup setup
    public void insertImageOnLeftTop(string sImagePath, int heightInExcelUnit)
    {
        //string sImagePath = "E:/Projetos/DOTNET/Deztaque/images/Logo_Rock_dourado.jpg";
        DrawingsPart dp = this._worksheetPart.AddNewPart<DrawingsPart>();
        ImagePart imgp = dp.AddImagePart(ImagePartType.Jpeg, this._worksheetPart.GetIdOfPart(dp));

        using (FileStream fs = new FileStream(sImagePath, FileMode.Open))
        {
            imgp.FeedData(fs);
        }

        /*****/
        NonVisualDrawingProperties nvdp = new NonVisualDrawingProperties();
        nvdp.Id = 1025;
        nvdp.Name = "Picture 1";
        nvdp.Description = "Header Image";
        DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
        picLocks.NoChangeAspect = true;
        picLocks.NoChangeArrowheads = true;
        NonVisualPictureDrawingProperties nvpdp = new NonVisualPictureDrawingProperties();
        nvpdp.PictureLocks = picLocks;
        NonVisualPictureProperties nvpp = new NonVisualPictureProperties();
        nvpp.NonVisualDrawingProperties = nvdp;
        nvpp.NonVisualPictureDrawingProperties = nvpdp;

        DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
        stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

        BlipFill blipFill = new BlipFill();
        DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
        blip.Embed = dp.GetIdOfPart(imgp);
        blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print;
        blipFill.Blip = blip;
        blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
        blipFill.Append(stretch);

        DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
        DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
        offset.X = 0;
        offset.Y = 0;
        t2d.Offset = offset;
        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(sImagePath);
        DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
        extents.Cx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
        extents.Cy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
        bm.Dispose();
        t2d.Extents = extents;
        ShapeProperties sp = new ShapeProperties();
        sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
        sp.Transform2D = t2d;
        DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
        prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
        prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
        sp.Append(prstGeom);
        sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

        DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
        picture.NonVisualPictureProperties = nvpp;
        picture.BlipFill = blipFill;
        picture.ShapeProperties = sp;

        Position pos = new Position();
        pos.X = 1828000;
        pos.Y = 45700;
        Extent ext = new Extent();
        ext.Cx = extents.Cx;
        ext.Cy = extents.Cy;
        AbsoluteAnchor anchor = new AbsoluteAnchor();
        anchor.Position = pos;
        anchor.Extent = ext;
        anchor.Append(picture);
        anchor.Append(new ClientData());
        WorksheetDrawing wsd = new WorksheetDrawing();
        wsd.Append(anchor);
        Drawing drawing = new Drawing();
        drawing.Id = dp.GetIdOfPart(imgp);

        wsd.Save(dp);

        this._worksheet.Append(drawing);

        setRowHeight(1, heightInExcelUnit);

        this._worksheet.Save();

        this._workbook.Save();
    }

    //Diagram diagram, ImageSetup setup
    public void insertImageOnPosition(string sImagePath, int heightInExcelUnit, double posX, double posY)
    {
        //string sImagePath = "E:/Projetos/DOTNET/Deztaque/images/Logo_Rock_dourado.jpg";
        //ImagePart imgp;
        if (this._drawingsPart == null)
        {
            this._drawingsPart = this._worksheetPart.AddNewPart<DrawingsPart>();
            this._imagePart = this._drawingsPart.AddImagePart(ImagePartType.Jpeg, this._worksheetPart.GetIdOfPart(this._drawingsPart));
            this._worksheetDrawing = new WorksheetDrawing();
        }
        else
        {
            this._imagePart = this._drawingsPart.AddImagePart(ImagePartType.Jpeg);
            this._drawingsPart.CreateRelationshipToPart(this._imagePart);
            //wsd = dp.WorksheetDrawing;
        }

        using (FileStream fs = new FileStream(sImagePath, FileMode.Open))
        {
            this._imagePart.FeedData(fs);
        }

        /*int imageNumber = this._drawingsPart.ImageParts.Count<ImagePart>();
        if (imageNumber == 1)
        {
            Drawing drawing = new Drawing();
            drawing.Id = this._drawingsPart.GetIdOfPart(this._imagePart);
            this._worksheet.Append(drawing);
        }*/

        /*****/
        NonVisualDrawingProperties nvdp = new NonVisualDrawingProperties();
        nvdp.Id = 1025;
        nvdp.Name = "Picture 1";
        nvdp.Description = "Header Image";
        DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
        picLocks.NoChangeAspect = true;
        picLocks.NoChangeArrowheads = true;
        NonVisualPictureDrawingProperties nvpdp = new NonVisualPictureDrawingProperties();
        nvpdp.PictureLocks = picLocks;
        NonVisualPictureProperties nvpp = new NonVisualPictureProperties();
        nvpp.NonVisualDrawingProperties = nvdp;
        nvpp.NonVisualPictureDrawingProperties = nvpdp;

        DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
        stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

        BlipFill blipFill = new BlipFill();
        DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
        blip.Embed = this._drawingsPart.GetIdOfPart(this._imagePart);
        blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print;
        blipFill.Blip = blip;
        blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
        blipFill.Append(stretch);

        DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
        DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
        offset.X = 0;
        offset.Y = 0;
        t2d.Offset = offset;
        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(sImagePath);
        DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
        extents.Cx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
        extents.Cy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
        bm.Dispose();
        t2d.Extents = extents;
        ShapeProperties sp = new ShapeProperties();
        sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
        sp.Transform2D = t2d;
        DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
        prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
        prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
        sp.Append(prstGeom);
        sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

        DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
        picture.NonVisualPictureProperties = nvpp;
        picture.BlipFill = blipFill;
        picture.ShapeProperties = sp;

        Position pos = new Position();
        pos.X = (long)posX;
        pos.Y = (long)posY;
        Extent ext = new Extent();
        ext.Cx = extents.Cx;
        ext.Cy = extents.Cy;
        AbsoluteAnchor anchor = new AbsoluteAnchor();
        anchor.Position = pos;
        anchor.Extent = ext;
        anchor.Append(picture);
        anchor.Append(new ClientData());

        this._worksheetDrawing.Append(anchor);
        Drawing drawing = new Drawing();
        drawing.Id = this._drawingsPart.GetIdOfPart(this._imagePart);

        this._worksheetDrawing.Save(this._drawingsPart);

        this._worksheet.Append(drawing);

        setRowHeight(1, heightInExcelUnit);

        this._worksheet.Save();

        this._workbook.Save();
    }

    //Diagram diagram, ImageSetup setup
    public void insertImageOnPositionFooter(string sImagePath, int rowIndex, int heightInExcelUnit, double posX, double posY)
    {
        //string sImagePath = "E:/Projetos/DOTNET/Deztaque/images/Logo_Rock_dourado.jpg";
        //ImagePart imgp;
        if (this._drawingsPart == null)
        {
            this._drawingsPart = this._worksheetPart.AddNewPart<DrawingsPart>();
            this._imagePart = this._drawingsPart.AddImagePart(ImagePartType.Jpeg, this._worksheetPart.GetIdOfPart(this._drawingsPart));
            this._worksheetDrawing = new WorksheetDrawing();
        }
        else
        {
            this._imagePart = this._drawingsPart.AddImagePart(ImagePartType.Jpeg);
            this._drawingsPart.CreateRelationshipToPart(this._imagePart);
            //wsd = dp.WorksheetDrawing;
        }

        using (FileStream fs = new FileStream(sImagePath, FileMode.Open))
        {
            this._imagePart.FeedData(fs);
        }

        /*int imageNumber = this._drawingsPart.ImageParts.Count<ImagePart>();
        if (imageNumber == 1)
        {
            Drawing drawing = new Drawing();
            drawing.Id = this._drawingsPart.GetIdOfPart(this._imagePart);
            this._worksheet.Append(drawing);
        }*/

        /*****/
        NonVisualDrawingProperties nvdp = new NonVisualDrawingProperties();
        nvdp.Id = 1025;
        nvdp.Name = "Picture 1";
        nvdp.Description = "Header Image";
        DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
        picLocks.NoChangeAspect = true;
        picLocks.NoChangeArrowheads = true;
        NonVisualPictureDrawingProperties nvpdp = new NonVisualPictureDrawingProperties();
        nvpdp.PictureLocks = picLocks;
        NonVisualPictureProperties nvpp = new NonVisualPictureProperties();
        nvpp.NonVisualDrawingProperties = nvdp;
        nvpp.NonVisualPictureDrawingProperties = nvpdp;

        DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
        stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

        BlipFill blipFill = new BlipFill();
        DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
        blip.Embed = this._drawingsPart.GetIdOfPart(this._imagePart);
        blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print;
        blipFill.Blip = blip;
        blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
        blipFill.Append(stretch);

        DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
        DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
        offset.X = 0;
        offset.Y = 0;
        t2d.Offset = offset;
        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(sImagePath);
        DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
        extents.Cx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
        extents.Cy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
        bm.Dispose();
        t2d.Extents = extents;
        ShapeProperties sp = new ShapeProperties();
        sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
        sp.Transform2D = t2d;
        DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
        prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
        prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
        sp.Append(prstGeom);
        sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

        DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
        picture.NonVisualPictureProperties = nvpp;
        picture.BlipFill = blipFill;
        picture.ShapeProperties = sp;

        Position pos = new Position();
        pos.X = (long)posX;
        pos.Y = (long)posY;
        Extent ext = new Extent();
        ext.Cx = extents.Cx;
        ext.Cy = extents.Cy;
        AbsoluteAnchor anchor = new AbsoluteAnchor();
        anchor.Position = pos;
        anchor.Extent = ext;
        anchor.Append(picture);
        anchor.Append(new ClientData());

        this._worksheetDrawing.Append(anchor);
        Drawing drawing = new Drawing();
        drawing.Id = this._drawingsPart.GetIdOfPart(this._imagePart);

        this._worksheetDrawing.Save(this._drawingsPart);

        this._worksheet.Append(drawing);

        setRowHeight(rowIndex, heightInExcelUnit);

        this._worksheet.Save();

        this._workbook.Save();
    }

    public Row CreateHeader(UInt32 index, string headerText)
    {
        Row r = new Row();
        r.RowIndex = index;

        Cell c = new Cell();
        c.DataType = CellValues.String;
        c.StyleIndex = 5;
        c.CellReference = "A" + index.ToString();
        c.CellValue = new CellValue(headerText);
        r.Append(c);

        this._sheetData.Append(r);

        return r;
    }

    public Row CreateHeader(UInt32 index, string headerText, int fontSize, int mergeColumns = 0)
    {
        Row r = new Row();
        r.RowIndex = index;

        Cell c = new Cell();
        c.DataType = CellValues.String;


        c.StyleIndex = 5;

        CellFormat x = (CellFormat)this._stylesSheet.CellFormats.ElementAt(5);
        x.Alignment = new Alignment();
        x.Alignment.Horizontal = HorizontalAlignmentValues.Left;
        Font y = (Font)this._stylesSheet.Fonts.ElementAt(int.Parse(x.FontId));
        y.FontSize = new FontSize() { Val = fontSize };

        c.CellReference = "A" + index.ToString();
        c.CellValue = new CellValue(headerText);

        if (mergeColumns > 0)
        {
            mergeCells("A" + index, formatValue(mergeColumns) + index);
        }

        r.Append(c);

        this._sheetData.Append(r);

        return r;
    }

    public void CreateColumnHeader(UInt32 index, IList<string> camposHeader)
    {
        //index = 1;
        Row r = new Row();
        r.RowIndex = index;

        int col = 0;

        foreach (var campo in camposHeader)
        {
            Cell c;
            c = new Cell();
            c.DataType = CellValues.String;
            //c.StyleIndex = 6;
            c.CellReference = formatValue(col) + index.ToString();
            c.CellValue = new CellValue(campo);
            c.StyleIndex = 1;

            r.Append(c);
            col++;
        }

        this._sheetData.Append(r);
    }

    public void CreateColumnHeader(UInt32 index, IList<string> camposHeader, bool mergeFirstRow)
    {
        //index = 1;
        Row r = new Row();
        r.RowIndex = index;

        int col = 0;

        foreach (var campo in camposHeader)
        {
            Cell c;
            c = new Cell();
            c.DataType = CellValues.String;
            //c.StyleIndex = 6;
            c.CellReference = formatValue(col) + index.ToString();
            c.CellValue = new CellValue(campo);
            c.StyleIndex = 1;
            r.Append(c);

            col++;
        }

        if (mergeFirstRow)
            mergeCells("A1", formatValue(col - 1) + "1");

        this._sheetData.Append(r);
    }

    // convert 0 to A, 1 to B, ... 17576 to ZZZ
    private string formatValue(int i)
    {
        var result = new StringBuilder();

        result.Insert(0, (char)('A' + (i % 26)));
        i -= 26;
        if (i > 0)
        {
            result.Insert(0, (char)('A' + (i % 26)));
            i -= 26;
            if (i > 0)
                result.Insert(0, (char)('A' + (i % 26)));
        }

        return result.ToString();
    }

    public void CreateRowWithStringContent(UInt32 index, string[] valores)
    {
        Row r = new Row();
        r.RowIndex = index;

        for (int i = 0; i < valores.Length; i++)
        {
            Cell c;
            c = new Cell();
            c.CellReference = formatValue(i) + index.ToString();
            //c.CellValue = new CellValue(rd.Next(1000000000).ToString("d9"));
            c.CellValue = new CellValue(valores[i]);
            c.StyleIndex = 6;
            c.DataType = CellValues.String;
            r.Append(c);
        }

        this._sheetData.Append(r);
    }

    public void CreateRowWithContent(UInt32 index, IList<string> valores, IList<int> tipo)
    {
        if (valores.Count == tipo.Count)
        {
            Row r = new Row();
            r.RowIndex = index;

            for (int i = 0; i < valores.Count; i++)
            {
                Cell c;
                c = new Cell();
                c.CellReference = formatValue(i) + index.ToString();
                //c.CellValue = new CellValue(rd.Next(1000000000).ToString("d9"));
                c.CellValue = new CellValue(valores[i]);
                c.StyleIndex = 6;
                switch (tipo[i])
                {
                    case TYPE_STRING:
                        c.DataType = CellValues.String;
                        break;
                    case TYPE_SHARED_STRING:
                        c.DataType = CellValues.SharedString;
                        break;
                    case TYPE_INLINE_STRING:
                        c.DataType = CellValues.InlineString;
                        break;
                    case TYPE_NUMBER:
                        c.DataType = CellValues.Number;
                        break;
                    case TYPE_DATE:
                        c.DataType = CellValues.Date;
                        break;
                    case TYPE_BOOLEAN:
                        c.DataType = CellValues.Boolean;
                        break;
                    case TYPE_ERROR:
                        c.DataType = CellValues.Error;
                        break;
                }

                r.Append(c);
            }

            /*DateTime dtEpoch = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            DateTime dt = dtEpoch.AddDays(rd.NextDouble() * 100000.0);
            TimeSpan ts = dt - dtEpoch;
            double fExcelDateTime;
            // Excel has "bug" of treating 29 Feb 1900 as valid
            // 29 Feb 1900 is 59 days after 1 Jan 1900, so just skip to 1 Mar 1900
            if (ts.Days >= 59)
            {
                fExcelDateTime = ts.TotalDays + 2.0;
            }
            else
            {
                fExcelDateTime = ts.TotalDays + 1.0;
            }
            c = new Cell();
            c.StyleIndex = 1;
            c.CellReference = "B" + index.ToString();
            c.CellValue = new CellValue(fExcelDateTime.ToString());
            r.Append(c);

            c = new Cell();
            c.StyleIndex = 2;
            c.CellReference = "C" + index.ToString();
            c.CellValue = new CellValue(((double)rd.Next(10, 10000000) + rd.NextDouble()).ToString("f4"));
            r.Append(c);

            c = new Cell();
            c.StyleIndex = 3;
            c.CellReference = "D" + index.ToString();
            c.CellValue = new CellValue(((double)rd.Next(10, 10000) + rd.NextDouble()).ToString("f2"));
            r.Append(c);

            c = new Cell();
            c.StyleIndex = 7;
            c.CellReference = "E" + index.ToString();
            c.CellValue = new CellValue(((double)rd.Next(10, 1000) + rd.NextDouble()).ToString("f2"));
            r.Append(c);*/


            this._sheetData.Append(r);
        }
    }

    public void CreateSummary(UInt32 index, IList<string> valores, IList<int> tipo)
    {
        if (valores.Count == tipo.Count)
        {
            Row r = new Row();
            r.RowIndex = index;

            for (int i = 0; i < valores.Count; i++)
            {
                Cell c;
                c = new Cell();
                c.CellReference = formatValue(i) + index.ToString();
                if (tipo[i] == TYPE_FORMULA)
                {
                    c.DataType = new EnumValue<CellValues>(CellValues.Number);
                    c.CellFormula = new CellFormula(valores[i]);
                    c.StyleIndex = 6;
                }
                else
                {
                    //c.CellValue = new CellValue(rd.Next(1000000000).ToString("d9"));
                    c.CellValue = new CellValue(valores[i]);
                    c.StyleIndex = 6;
                    switch (tipo[i])
                    {
                        case TYPE_STRING:
                            c.DataType = CellValues.String;
                            break;
                        case TYPE_SHARED_STRING:
                            c.DataType = CellValues.SharedString;
                            break;
                        case TYPE_INLINE_STRING:
                            c.DataType = CellValues.InlineString;
                            break;
                        case TYPE_NUMBER:
                            c.DataType = CellValues.Number;
                            break;
                        case TYPE_DATE:
                            c.DataType = CellValues.Date;
                            break;
                        case TYPE_BOOLEAN:
                            c.DataType = CellValues.Boolean;
                            break;
                        case TYPE_ERROR:
                            c.DataType = CellValues.Error;
                            break;
                    }
                }

                r.Append(c);
            }

            this._sheetData.Append(r);
        }
    }


    public void save()
    {
        this._workbook.Save();
    }

    public void setRowHeight(int rowIndex, int height)
    {
        Row row;

        try
        {
            row = worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowIndex).First();

            row.Height = height;

            row.CustomHeight = true;
        }
        catch (Exception ex)
        {
            row = new Row();// { RowIndex = (uint)rowIndex, Height = height, CustomHeight = true };
            row.RowIndex = (uint)rowIndex;
            row.Height = height;
            row.CustomHeight = true;

            Cell cell = new Cell();// { CellReference = "A" + rowIndex.ToString(), CellValue = new CellValue("teste"), DataType = CellValues.String };
            cell.DataType = CellValues.String;
            cell.CellReference = formatValue(0) + rowIndex.ToString();
            cell.CellValue = new CellValue("");

            row.Append(cell);

            sheetData.Append(row);
        }

        this.save();
    }

    public void mergeCells(string firstRangeCell, string lastRangeCell)
    {
        MergeCells mergeCells;

        if (worksheet.Elements<MergeCells>().Count() > 0)
        {
            mergeCells = worksheet.Elements<MergeCells>().First();
        }
        else
        {
            mergeCells = new MergeCells();

            // Insert a MergeCells object into the specified position.
            if (this._worksheet.Elements<CustomSheetView>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<CustomSheetView>().First());
            }
            else if (this._worksheet.Elements<DataConsolidate>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<DataConsolidate>().First());
            }
            else if (this._worksheet.Elements<SortState>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<SortState>().First());
            }
            else if (this._worksheet.Elements<AutoFilter>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<AutoFilter>().First());
            }
            else if (this._worksheet.Elements<Scenarios>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<Scenarios>().First());
            }
            else if (this._worksheet.Elements<ProtectedRanges>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<ProtectedRanges>().First());
            }
            else if (this._worksheet.Elements<SheetProtection>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<SheetProtection>().First());
            }
            else if (this._worksheet.Elements<SheetCalculationProperties>().Count() > 0)
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<SheetCalculationProperties>().First());
            }
            else
            {
                this._worksheet.InsertAfter(mergeCells, this._worksheet.Elements<SheetData>().First());
            }
        }

        MergeCell mergeCell = new MergeCell() { Reference = new StringValue(firstRangeCell + ":" + lastRangeCell) };
        mergeCells.Append(mergeCell);

        this._worksheet.Save();
    }

    public void setColumnWidth(uint firstRangeCol, uint lastRangeCol, int width)
    {
        Column xlCol = new Column();
        xlCol.Width = width;
        xlCol.BestFit = true;
        xlCol.CustomWidth = true;
        xlCol.Min = firstRangeCol;
        xlCol.Max = lastRangeCol;
        this._columns.Append(xlCol);

        this._worksheet.Save();
    }

    public void setColumnWidth(uint column, int width)
    {
        Column xlCol = new Column();
        xlCol.Width = width;
        xlCol.BestFit = true;
        xlCol.CustomWidth = true;
        xlCol.Min = column;
        xlCol.Max = column;
        this._columns.Append(xlCol);

        this._worksheet.Save();
    }

    private Stylesheet generateStyleSheet()
    {
        return new Stylesheet(
            new Fonts(
                new Font(                                                               // Index 0 - The default font.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue("00000000") },
                    new FontName() { Val = "Calibri" }),
                new Font(                                                               // Index 1 - The bold font.
                    new Bold(),
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue("00000000") },
                    new FontName() { Val = "Calibri" }),
                new Font(                                                               // Index 2 - The Italic font.
                    new Italic(),
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue("00000000") },
                    new FontName() { Val = "Calibri" }),
                new Font(                                                               // Index 3 - The Times Roman font. with 16 size
                    new Bold(),
                    new FontSize() { Val = 28 },
                    new Color() { Rgb = new HexBinaryValue("00000000") },
                    new FontName() { Val = "Calibri" })
            ),
            new Fills(
                new Fill(                                                           // Index 0 - The default fill.
                    new PatternFill() { PatternType = PatternValues.None }),
                new Fill(                                                           // Index 1 - The default fill of gray 125 (required)
                    new PatternFill() { PatternType = PatternValues.Gray125 }),
                new Fill(                                                           // Index 2 - The yellow fill.
                    new PatternFill(
                        new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
                    ) { PatternType = PatternValues.Solid })
            ),
            new Borders(
                new Border(                                                         // Index 0 - The default border.
                    new LeftBorder(),
                    new RightBorder(),
                    new TopBorder(),
                    new BottomBorder(),
                    new DiagonalBorder()),
                new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                    new LeftBorder(
            /* new Color() { Auto = true } */
                    ) { Style = BorderStyleValues.Thin, Color = new Color() { Rgb = new HexBinaryValue("00000000") } },
                    new RightBorder(
            /* new Color() { Auto = true } */
                    ) { Style = BorderStyleValues.Thin, Color = new Color() { Rgb = new HexBinaryValue("00000000") } },
                    new TopBorder(
            /* new Color() { Auto = true } */
                    ) { Style = BorderStyleValues.Thin, Color = new Color() { Rgb = new HexBinaryValue("00000000") } },
                    new BottomBorder(
            /* new Color() { Auto = true } */
                    ) { Style = BorderStyleValues.Thin, Color = new Color() { Rgb = new HexBinaryValue("00000000") } },
                    new DiagonalBorder())
            ),
            new CellFormats(
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                new CellFormat() { FontId = 1, FillId = 0, BorderId = 1, ApplyFont = true },       // Index 1 - Bold 
                new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 - Italic
                new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 - Times Roman
                new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Index 4 - Yellow Fill
                new CellFormat(                                                                   // Index 5 - Alignment
                    new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                ) { FontId = 3, FillId = 0, BorderId = 0, ApplyAlignment = true },
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 - Border
            )
        ); // return
    }

    /*
     * 1emu = ( 1 / 360000 )cm
     * 
     * 1cm = 360000emu
     * */
    public static double convertCMToEMU(double value)
    {
        double result = 0;

        result = value * 360000;

        return result;
    }

    /*
     * 1emu = ( 1 / 914400 )in
     * 
     * 1in = 914400emu
     * */
    public static double convertInchsToEMU(double value)
    {
        double result = 0;

        result = value * 914400;

        return result;
    }

    /*
     * 1in = 13excelunits
     * 
     * 1excelunit = 1/13in
     * 
     * 1emu = (1/11887200)excelunits
     * 
     * 1excelunit = 11887200emu
     * */
    public static double convertExcelPointsToEMU(double value)
    {
        double result = 0;

        result = value / 13.71;

        result = convertInchsToEMU(result);

        /*  result = (result) * 914400; */

        //result = convertInchsToEMU(result);

        return result;
    }

    public static void InsertImage(Worksheet ws, long x, long y, long? width, long? height, string sImagePath)
    {
        try
        {
            WorksheetPart wsp = ws.WorksheetPart;
            DrawingsPart dp;
            ImagePart imgp;
            WorksheetDrawing wsd;

            ImagePartType ipt;
            switch (sImagePath.Substring(sImagePath.LastIndexOf('.') + 1).ToLower())
            {
                case "png":
                    ipt = ImagePartType.Png;
                    break;
                case "jpg":
                case "jpeg":
                    ipt = ImagePartType.Jpeg;
                    break;
                case "gif":
                    ipt = ImagePartType.Gif;
                    break;
                default:
                    return;
            }

            if (wsp.DrawingsPart == null)
            {
                //----- no drawing part exists, add a new one

                dp = wsp.AddNewPart<DrawingsPart>();
                imgp = dp.AddImagePart(ipt, wsp.GetIdOfPart(dp));
                wsd = new WorksheetDrawing();
            }
            else
            {
                //----- use existing drawing part

                dp = wsp.DrawingsPart;
                imgp = dp.AddImagePart(ipt);
                dp.CreateRelationshipToPart(imgp);
                wsd = dp.WorksheetDrawing;
            }

            using (FileStream fs = new FileStream(sImagePath, FileMode.Open))
            {
                imgp.FeedData(fs);
            }

            int imageNumber = dp.ImageParts.Count<ImagePart>();
            if (imageNumber == 1)
            {
                Drawing drawing = new Drawing();
                drawing.Id = dp.GetIdOfPart(imgp);
                ws.Append(drawing);
            }

            NonVisualDrawingProperties nvdp = new NonVisualDrawingProperties();
            nvdp.Id = new UInt32Value((uint)(1024 + imageNumber));
            nvdp.Name = "Picture " + imageNumber.ToString();
            nvdp.Description = "";
            DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
            picLocks.NoChangeAspect = true;
            picLocks.NoChangeArrowheads = true;
            NonVisualPictureDrawingProperties nvpdp = new NonVisualPictureDrawingProperties();
            nvpdp.PictureLocks = picLocks;
            NonVisualPictureProperties nvpp = new NonVisualPictureProperties();
            nvpp.NonVisualDrawingProperties = nvdp;
            nvpp.NonVisualPictureDrawingProperties = nvpdp;

            DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
            stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

            BlipFill blipFill = new BlipFill();
            DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
            blip.Embed = dp.GetIdOfPart(imgp);
            blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print;
            blipFill.Blip = blip;
            blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
            blipFill.Append(stretch);

            DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
            DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
            offset.X = 0;
            offset.Y = 0;
            t2d.Offset = offset;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(sImagePath);

            DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();

            if (width == null)
                extents.Cx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
            else
                extents.Cx = width;

            if (height == null)
                extents.Cy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
            else
                extents.Cy = height;

            bm.Dispose();
            t2d.Extents = extents;
            ShapeProperties sp = new ShapeProperties();
            sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
            sp.Transform2D = t2d;
            DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
            prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
            prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
            sp.Append(prstGeom);
            sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

            DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
            picture.NonVisualPictureProperties = nvpp;
            picture.BlipFill = blipFill;
            picture.ShapeProperties = sp;

            Position pos = new Position();
            pos.X = x;
            pos.Y = y;
            Extent ext = new Extent();
            ext.Cx = extents.Cx;
            ext.Cy = extents.Cy;
            AbsoluteAnchor anchor = new AbsoluteAnchor();
            anchor.Position = pos;
            anchor.Extent = ext;
            anchor.Append(picture);
            anchor.Append(new ClientData());
            wsd.Append(anchor);
            wsd.Save(dp);
        }
        catch (Exception ex)
        {
            throw ex; // or do something more interesting if you want
        }
    }

    public static void InsertImage(Worksheet ws, long x, long y, string sImagePath)
    {
        InsertImage(ws, x, y, null, null, sImagePath);
    }
}