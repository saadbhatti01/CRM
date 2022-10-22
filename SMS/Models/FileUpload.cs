using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public class FileUpload
{
    //This function is used by the uploadFile() function below
    public bool getFileExtension(string reqExtensions, string fileName)
    {
        string[] result = new string[2];
        result = fileName.Split('.');
        //result[1] contains the second part (extension) as result[0] contains the filename
        //the extensions contain all the required file extensions which are comma seperated 
        string[] extensions = reqExtensions.Split(',');
        for (int i = 0; i < extensions.Length; i++)
        {
            if (extensions[i] == result[1])
                return true;
        }
        return false;
    }

    //This function will be called from main page
    public bool uploadFile(long reqFileSize, long fileSize, string reqExtensions, string fileName)
    {
        bool result = getFileExtension(reqExtensions, fileName);
        if (result == false)
            return false;
        else if (fileSize > reqFileSize)
            return false;
        else
            return true;
    }

    //Delete a File
    public void DeleteFile(string path)
    {
        File.Delete(path);
    }

    //Thumbs
    public void makeThumbnail(int width, int height, string FilePath, string UpFileName)
    {
        //making thumbnail...
        System.Drawing.Image imag;
        imag = System.Drawing.Image.FromFile(FilePath + UpFileName);
        System.Drawing.Image Thumbnailimag;
        System.Drawing.Image Cropedimag;
        //System.Drawing.Image Resizedimag;
        Size ThumbSize = new Size();
        //Size imagSize = new Size();
        ThumbSize.Width = width;
        ThumbSize.Height = height;

        FileUpload resizeimage = new FileUpload();
        Rectangle rectan = new Rectangle();

        if (imag.Height > imag.Width)
        {
            rectan.Height = imag.Width;
            rectan.Width = imag.Width;
            Point point = new Point();
            point.Y = (imag.Height - imag.Width) / 2;
            rectan.Offset(point);
            Cropedimag = resizeimage.cropImage(imag, rectan);
            Thumbnailimag = resizeimage.resize(Cropedimag, ThumbSize);
            //UpFileName = resizeimage.Get_Unique_FileName(FilePath, UpFileName);
            Thumbnailimag.Save(FilePath + "Thumbnail_" + UpFileName);
            imag.Dispose();
        }
        else if (imag.Height <= imag.Width)
        {
            rectan.Width = imag.Height;
            rectan.Height = imag.Height;
            Point point = new Point();
            point.X = (imag.Width - imag.Height) / 2;
            rectan.Offset(point);
            Cropedimag = resizeimage.cropImage(imag, rectan);
            Thumbnailimag = resizeimage.resize(Cropedimag, ThumbSize);
            //UpFileName = resizeimage.Get_Unique_FileName(FilePath, UpFileName);
            Thumbnailimag.Save(FilePath + "Thumbnail_" + UpFileName);
            imag.Dispose();
        }
    }

    //Large
    public void makeLarge(int width, int height, string FilePath, string UpFileName)
    {
        //making thumbnail...
        System.Drawing.Image imag;
        imag = System.Drawing.Image.FromFile(FilePath + UpFileName);
        System.Drawing.Image Thumbnailimag;
        System.Drawing.Image Cropedimag;
        //System.Drawing.Image Resizedimag;
        Size ThumbSize = new Size();
        //Size imagSize = new Size();
        ThumbSize.Width = width;
        ThumbSize.Height = height;

        FileUpload resizeimage = new FileUpload();
        Rectangle rectan = new Rectangle();

        if (imag.Height > imag.Width)
        {
            rectan.Height = imag.Width;
            rectan.Width = imag.Width;
            Point point = new Point();
            point.Y = (imag.Height - imag.Width) / 2;
            rectan.Offset(point);
            Cropedimag = resizeimage.cropImage(imag, rectan);
            Thumbnailimag = resizeimage.resize(Cropedimag, ThumbSize);
            //UpFileName = resizeimage.Get_Unique_FileName(FilePath, UpFileName);
            Thumbnailimag.Save(FilePath + "Large_" + UpFileName);
            imag.Dispose();
        }
        else if (imag.Height <= imag.Width)
        {
            rectan.Width = imag.Height;
            rectan.Height = imag.Height;
            Point point = new Point();
            point.X = (imag.Width - imag.Height) / 2;
            rectan.Offset(point);
            Cropedimag = resizeimage.cropImage(imag, rectan);
            Thumbnailimag = resizeimage.resize(Cropedimag, ThumbSize);
            //UpFileName = resizeimage.Get_Unique_FileName(FilePath, UpFileName);
            Thumbnailimag.Save(FilePath + "Large_" + UpFileName);
            imag.Dispose();
        }
    }

    public System.Drawing.Image resize(System.Drawing.Image imgToResize, Size size)
    {
        int sourceWidth = imgToResize.Width;
        int sourceHeight = imgToResize.Height;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)size.Width / (float)sourceWidth);
        nPercentH = ((float)size.Height / (float)sourceHeight);

        if (nPercentH < nPercentW)
            nPercent = nPercentH;
        else
            nPercent = nPercentW;

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage((System.Drawing.Image)b);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();

        return (System.Drawing.Image)b;
    }

    //Stores File with unique name
    public string Get_Unique_FileName(string filePath, string fileName)
    {
        string imgName, imgExt;
        string renamedFile = "";
        int renameCount = 0;
        string newFileName = "";

        string[] NameExtArray;
        NameExtArray = fileName.Split('.');
        imgName = NameExtArray[0];
        imgExt = NameExtArray[1];

        while (File.Exists(filePath + imgName + renamedFile + "." + imgExt))
        {
            renameCount += 1;
            renamedFile = renameCount.ToString();
        }

        newFileName = imgName + renamedFile + "." + imgExt;
        return newFileName;
    }

    public System.Drawing.Image cropImage(System.Drawing.Image img, Rectangle cropArea)
    {
        Bitmap bmpImage = new Bitmap(img);
        Bitmap bmpCrop = bmpImage.Clone(cropArea,
        bmpImage.PixelFormat);

        bmpImage.Dispose();
        return (System.Drawing.Image)(bmpCrop);
    }
}