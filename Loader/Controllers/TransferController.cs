using iTextSharp.text;
using iTextSharp.text.pdf;
using Loader.EntityDataModel;
using Loader.Models;
using Loader.Pdf;
using Loader.Service;
using Loader.ViewModel;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static Loader.ViewModel.Metadata;

namespace Loader.Controllers
{
    public class TransferController : Controller
    {
        private readonly TransferService _storeTranService;
        public TransferController()
        {
            _storeTranService = new TransferService();
        }

        // GET: Transfer
        public ActionResult Storetransfer()
        {
            var item = _storeTranService.GetAllStoreTransferList();
            return View(item);
        }

        [HttpGet]
        public ActionResult RequestDetails(int? RMId)
        {
            if(RMId != null)
            {
                ViewBag.Function = "Transfer";
                var storePenList = _storeTranService.GetStoreTransferPendingList(RMId);
                return View(storePenList);
            }
            return View();
        }

        public ActionResult _AddNewItem()
        {
            return PartialView();
        }
       
        public ActionResult _StoreRemarks()
        {
            return PartialView();
        }

        public ActionResult _GetIndividualItemDetails(int STDId)
        {
            return PartialView(_storeTranService.GetAllMacSerialNo(STDId));
        }

        public ActionResult GetItemDetailView(int numberOfRows,int ItemId)
        {
            ViewBag.numberOfRows = numberOfRows;
            FgetPurchaseSerialNolist_ResultMetaData returnItemDetailSerialMac = new FgetPurchaseSerialNolist_ResultMetaData();
            returnItemDetailSerialMac.ItemId = ItemId;
            return PartialView(returnItemDetailSerialMac);
        }

        //reject store transfer
        public ActionResult StoreRejectRequest(int RdId)
        {
            var result = _storeTranService.StoreRejectItemRequestAndReturnRMId(RdId);
            return PartialView(result);
        }
        

        //IEnumerable<FgetStoreTransferItemPening_ResultMetadata> RequestDetailsList
        //[HttpGet]
        //public ActionResult StoreTransfer()
        //{
        //var res = _storeTranService.GetRquestDetails();
        //  return View();
        //}


        //[HttpPost]
        //public ActionResult StoreTransfer(FgetPendinStoreTransfer_Result storeTrans, List<FgetStoreTransferItemPening_ResultMetadata> RequestDetailsList, List<FgetPurchaseSerialNolist_ResultMetaData> itemDetailsList)
        //{
        //    if (RequestDetailsList == null || !RequestDetailsList.Any())
        //    {
        //        return Json(new { success = false, message = "RequestDetailsList cannot be null or empty." }, JsonRequestBehavior.AllowGet);
        //    }

        //    var STMId = _storeTranService.SaveStoreTransferDetails(storeTrans, RequestDetailsList, itemDetailsList);
        //    if (STMId == 0)
        //    {
        //        return Json("StockOut", JsonRequestBehavior.AllowGet);
        //    }
        //    return PrintGatePassVersionV2(STMId);
        //}


        [HttpPost]
        //FgetPendinStoreTransfer_Result storeTrans, List<FgetStoreTransferItemPening_ResultMetadata> RequestDetailsList, List<FgetPurchaseSerialNolist_ResultMetaData> itemDetailsList
        public ActionResult StoreTransfer(FgetPendinStoreTransfer_Result storeTrans, FgetStoreTransferItemPening_ResultMetadata RequestDetailsList, FgetPurchaseSerialNolist_ResultMetaData itemDetailsList)
        {
            var STMId = _storeTranService.SaveStoreTransferDetails(storeTrans, RequestDetailsList, itemDetailsList);
            if (STMId == 0)
            {
                return Json("StockOut", JsonRequestBehavior.AllowGet);
            }
            return PrintGatePassVersionV2(STMId);
        }

        public JsonResult PrintGatePassVersionV2(int STMId)
        {
            try
            {
                var btd = _storeTranService.GetPassList(STMId);
                var fName = string.Format(DateTime.Now.ToString() + "-GatePass.pdf", DateTime.Now.ToString("s"));

                PdfPTable pdfTable = new PdfPTable(4);
                float[] headers = { 25, 25, 25, 25 };
                pdfTable.SetWidths(headers);
                pdfTable.WidthPercentage = 100;
                pdfTable.HeaderRows = 4;

                string MacNo = "";
                string SerialNo = "";

                using (MemoryStream stream = new MemoryStream())
                {
                    using (Document pdfDoc = new Document(PageSize.A4, 0f, 0f, 120f, 60f))
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        writer.PageEvent = new PdfEvents(STMId);

                        writer.CloseStream = false;
                        pdfDoc.Open();

                        float[] headers1 = { 10, 40, 15, 35 };
                        PdfPTable pdfPTable3 = new PdfPTable(4);
                        pdfPTable3.WidthPercentage = 85;

                        pdfPTable3.SetWidths(headers1); //Set the pdf headers
                                                        // pdfPTable3.SpacingBefore=pdfDoc.PageSize.Height - 750;
                        AddCellToHeader(pdfPTable3, "SNo.");
                        AddCellToHeader(pdfPTable3, "Item Name");
                        AddCellToHeader(pdfPTable3, "Quantity");
                        AddCellToHeader(pdfPTable3, "Remark");
                        pdfDoc.Add(pdfPTable3);

                        PdfPTable pdfPTable4 = new PdfPTable(4);
                        pdfPTable4.DefaultCell.Padding = 6;
                        pdfPTable4.WidthPercentage = 85;
                        pdfPTable4.SplitLate = false;
                        pdfPTable4.SetWidths(headers1); //Set the pdf headers

                        foreach (var stm in btd)
                        {
                            MacNo = "";
                            SerialNo = "";

                            if (stm.MacNo != null && stm.MacNo.Trim() != "")
                            {

                                MacNo = stm.MacNo;
                            }
                            if (stm.SerialNo != null && stm.SerialNo.Trim() != "")
                            {

                                SerialNo = stm.SerialNo;
                            }

                            AddCellToBody(pdfPTable4, Convert.ToString(stm.SNo), "", "");
                            AddCellToBody(pdfPTable4, stm.ItemName, MacNo, SerialNo);
                            AddCellToBody(pdfPTable4, Convert.ToString(stm.Qnty), "", "");
                            AddCellToBody(pdfPTable4, stm.StoreRemarks, "", "");
                        }

                        //  pdfDoc.SetMargins(0, 0, 750, 60);               
                        pdfDoc.Add(pdfPTable4);
                        pdfDoc.Close();
                    }
                    var bytes1 = stream.ToArray();
                    Session[fName] = bytes1;
                }

                return Json(new { success = true, fName }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddCellToBody(PdfPTable pdfPTable, string cellText, string MacNo, string SerialNo)
        {
            PdfPCell pdfPCell = new PdfPCell();

            pdfPCell.AddElement(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL, BaseColor.BLACK)));

            if(MacNo != "")
            {
                pdfPCell.AddElement(AddLabel(new Phrase(), MacNo, "Mac No : "));
            }
            if (SerialNo != "")
            {
                pdfPCell.AddElement(AddLabel(new Phrase(), SerialNo, "Serial No : "));

            }
            pdfPTable.AddCell(pdfPCell);
        }

        //  Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable pdfPTable, string cellText)
        {
            pdfPTable.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 11, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });

        }

        public static Phrase AddLabel(Phrase phrase, string cellText, string label)
        {
            var boldFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, 8);
            var normalFont = FontFactory.GetFont(FontFactory.COURIER, 8);
            phrase.Add(new Chunk(label, boldFont));
            phrase.Add(new Chunk(cellText, normalFont));
            return phrase;
        }

        public ActionResult DownloadInvoice(string fName)
        {
            var ms = Session[fName] as byte[];
            //System.IO.File.WriteAllBytes(@"~/Doc/readme.txt", ms);
            if (ms == null)
                return new EmptyResult();

            Session[fName] = null;
           // return File(ms, "application/octet-stream", fName);
            return File(ms, "application/pdf", fName);
        }


        public ActionResult AddNewField()
        {
            //ViewBag.Index = index;
            return PartialView("_GetItemDetailView");
        }

    }
}


