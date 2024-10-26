using ChannakyaCustomeDatePicker.Repository;
using DocumentFormat.OpenXml.Bibliography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Loader.DataAccess;
using Loader.ViewModel.GenerateGradeSheet;
using Loader.ViewModel.Grade;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Loader.Service
{
    public class GradeSheetService
    {
        private GenericUnitOfWork _uow = null;
        public GradeSheetService()
        {
            _uow = new GenericUnitOfWork();
        }

        public List<GradeSheetSetupVM> GetYearLists()
        {
            string query = "select [FYID] as FYId,[NYear] from fgetFiscalYeatNepali()";

            List<GradeSheetSetupVM> list = new List<GradeSheetSetupVM>();
            using(SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GradeSheetSetupVM f = new GradeSheetSetupVM()
                        {

                            FiscalYear = (long)reader["FYId"],
                            NYear = (int)reader["NYear"]
                        };
                        list.Add(f);
                    }
                    con.Close();
                    con.Dispose();
                    return list;
                }
            }
        }
        public List<GradeSheetSetupVM> GetGradeLists()
        {
            string query = $"select [Mgid],[GradeName] from [dbo].[FgetMasterGrade]() where statusId=1";

            List<GradeSheetSetupVM> list = new List<GradeSheetSetupVM>();
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GradeSheetSetupVM g = new GradeSheetSetupVM()
                        {
                            Mgid = (int)reader["Mgid"],
                            GradeName = (string)reader["GradeName"]
                        };
                        list.Add(g);
                    }
                    con.Close();
                    con.Dispose();
                    return list;
                }
            }
        }

        public List<GradeSheetSetupVM> GetSectionsLists(int id)
        {
            string query = $"select SecId,SectionName from FgetSectionList() where (StatusId = 1 and [ClassId] = {id})";
            List<GradeSheetSetupVM> list = new List<GradeSheetSetupVM>();
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GradeSheetSetupVM g = new GradeSheetSetupVM()
                        {
                            sectionId = (int)reader["SecId"],
                            SectionName = (string)reader["SectionName"]
                        };
                        list.Add(g);
                    }
                    con.Close();
                    con.Dispose();
                    return list;
                }
            }
            /*string query = $"select SecId,SectionName from FgetSectionList() where (StatusId = 1 and [ClassId] = {id} )";
            List<GradeSheetSetupVM> lists = uow
                                .Repository<GradeSheetSetupVM>().
                                ExecWithStoreProcedure(query)
                                .Select(x => new GradeSheetSetupVM
                                {
                                    SectionId = x.SecId,
                                    SectionName = x.SectionName

                                }).ToList();
            return lists;*/
        }

        public List<GradeSheetSetupVM> GetExamTerm()
        {
            string query = "select [ETermId],[TermName] from MasterExamTerm ";
            List<GradeSheetSetupVM> list = new List<GradeSheetSetupVM>();
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GradeSheetSetupVM f = new GradeSheetSetupVM()
                        {
                            ETermId = (int)reader["ETermId"],
                            TermName = (string)reader["TermName"]
                        };
                        list.Add(f);
                    }
                    con.Close();
                    return list;
                }
            }
        }

        public List<GradeSheetSetupVM> GetStudentName(int id)
        {
            string query = $"select [Studentid],[StudentName] from FgetStudentGradeDetails() where GradeId = {id}";
            List<GradeSheetSetupVM> list = new List<GradeSheetSetupVM>();
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GradeSheetSetupVM f = new GradeSheetSetupVM()
                        {
                            StudentId = (int)reader["Studentid"],
                            StudentName = (string)reader["StudentName"]
                        };
                        list.Add(f);
                    }
                    con.Close();
                    return list;
                }
            }
        }

        public List<GradeSheetSetupVM> GetStudentDetail(int id)
        {
            string query = $"select [RollNo],[StudentName],[EnrollFy],[FacultyName] from FgetStudentGradeDetails() where StudentId ={id}";

            List<GradeSheetSetupVM> list = new List<GradeSheetSetupVM>();
            using(SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GradeSheetSetupVM g = new GradeSheetSetupVM()
                        {
                            RollNo = (string)reader["RollNo"],
                            StudentName = (string)reader["StudentName"],
                            EnrollFy = (int)reader["EnrollFy"],
                            FacultyName= (string)reader["FacultyName"]
                        };
                        list.Add(g);
                    }
                    con.Close();
                    return list;
                }
            }
        }

        public List<CompanyInfo> GetCompanyDetails()
        {
            string query = $"select [CompanyName],[CompanyAddress],[Estd] from FgetCompanyDetails()";

            List<CompanyInfo> list = new List<CompanyInfo>();
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CompanyInfo g = new CompanyInfo()
                        {
                            CompanyName = (string)reader["CompanyName"],
                            CompanyAddress = (string)reader["CompanyAddress"],
                            Estd = (string)reader["Estd"],
                        };
                        list.Add(g);
                    }
                    con.Close();
                    return list;
                }
            }
        }

        public List<GradeSheetSetupVM> GetCompanyInfo()
        {
            string query = "select * from FgetCompanyDetails()";
            var companyInfo = _uow.Repository<GradeSheetSetupVM>().ExecWithStoreProcedure(query).ToList();
            return companyInfo;
        }

        public List<GradeSheetSetupVM> GetGradeSheetByStudent(int gradeId, int sectionId, int examId, int fiscalYear, int studentId)
        {
            List<GradeSheetSetupVM> gradeSheet = new List<GradeSheetSetupVM>();
            try
            {
                string query = $"select * from FgetGrdeSheetByStudentPrint({gradeId},{sectionId},{examId},{fiscalYear},{studentId}) Order by convert(int,SubjectCode) ";
                gradeSheet = _uow.Repository<GradeSheetSetupVM>()
                    .ExecWithStoreProcedure(query).Select(s => new GradeSheetSetupVM
                    {
                        StudentId = s.StudentId,
                        RollNo = s.RollNo,
                        StudentName = s.StudentName,
                        RegistrationNo = s.RegistrationNo,
                        GradeId = s.GradeId,
                        GradeName = s.GradeName,
                        sectionId = s.sectionId,
                        FinalGrade = s.FinalGrade,
                        GPA = s.GPA,
                        Grade= s.Grade,
                        CreditHour = s.CreditHour,
                        SubjectCode = s.SubjectCode,
                        Subjects = s.Subjects,
                        GradePoint = s.GradePoint,
                        SectionName = s.SectionName,
                        FYId = s.FYId,
                        YearBS = s.YearBS,
                        YearAD = s.YearAD,
                        FacultyGroupName = s.FacultyGroupName,
                        DOBAD = s.DOBAD,
                        DOBBS = s.DOBBS
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the grade sheet.", ex);
            }
            return gradeSheet;
        }


        //----------ForPDFGENERATION----------

        public MemoryStream GenerateGradeSheetPdf(List<GradeSheetSetupVM> studentDetails, List<CompanyInfo> companyInfo)
        {
            if (studentDetails == null || !studentDetails.Any())
                throw new ArgumentException("Student details cannot be null or empty");

            if (companyInfo == null || !companyInfo.Any())
                throw new ArgumentException("Company information cannot be null or empty");

            MemoryStream stream = new MemoryStream();
            // Change page margins to match the image (appears to have narrower margins)
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25f, 25f, 25f, 25f);
            PdfWriter writer = null;

            try
            {
                writer = PdfWriter.GetInstance(document, stream);
                writer.CloseStream = false;
                document.Open();

                // Font setup
                BaseFont baseFont;
                try
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string fontPath = Path.Combine(baseDirectory, "fonts", "times.ttf");
                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch (Exception)
                {
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                }

                // Modified font sizes to match the image
                Font titleFont = new Font(baseFont, 16, Font.BOLD);
                Font headerFont = new Font(baseFont, 14, Font.BOLD);
                Font normalFont = new Font(baseFont, 12, Font.NORMAL);

                var student = studentDetails.FirstOrDefault();
                var company = companyInfo.FirstOrDefault();

                // Create header table for logo and school info
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 1f, 3f });

                // Logo cell (left side)
                PdfPCell logoCell = new PdfPCell();
                logoCell.Border = Rectangle.BOX;
                logoCell.BorderWidth = 1f;
                logoCell.BorderColor = BaseColor.BLUE;
                logoCell.MinimumHeight = 80f;
                /*if (!string.IsNullOrEmpty(company?.CompanyLogo))
                {
                    Image logo = Image.GetInstance(company.CompanyLogo);
                    logo.ScaleToFit(70f, 70f);
                    logoCell.AddElement(logo);
                }*/
                headerTable.AddCell(logoCell);

                // School info cell (right side)
                PdfPCell schoolInfoCell = new PdfPCell();
                schoolInfoCell.Border = Rectangle.BOX;
                schoolInfoCell.BorderWidth = 1f;
                schoolInfoCell.BorderColor = BaseColor.BLUE;

                // School name and details with center alignment
                Paragraph schoolInfo = new Paragraph();
                schoolInfo.Alignment = Element.ALIGN_CENTER;
                schoolInfo.Add(new Chunk((company?.CompanyName?.ToUpper() ?? "[SCHOOL NAME]") + "\n", headerFont));
                schoolInfo.Add(new Chunk((company?.CompanyAddress ?? "[Location]") + "\n", normalFont));
                schoolInfo.Add(new Chunk("ESTD : " + (company?.Estd ?? "[Date (B.S)]"), normalFont));
                schoolInfoCell.AddElement(schoolInfo);

                // Add GRADE-SHEET title
                Paragraph gradeSheetTitle = new Paragraph("GRADE~SHEET", titleFont);
                gradeSheetTitle.Alignment = Element.ALIGN_CENTER;
                gradeSheetTitle.SpacingBefore = 10f;
                schoolInfoCell.AddElement(gradeSheetTitle);

                headerTable.AddCell(schoolInfoCell);
                document.Add(headerTable);

                // Student details table
                PdfPTable studentTable = new PdfPTable(1);
                studentTable.WidthPercentage = 100;
                studentTable.SpacingBefore = 20f;

                // The Grade(s) Secured by
                AddStudentDetailRow(studentTable, "THE GRADE(S) SECURED BY : ", student?.StudentName ?? "", normalFont);

                // Date of Birth
                string dobText = $"DATE OF BIRTH : {student?.DOBBS ?? ""} B.S ( {student?.DOBAD:dd-MM-yyyy} A.D )";
                AddStudentDetailRow(studentTable, dobText, "", normalFont);

                // Registration and Symbol Number
                string regSymbolText = $"REGISTRATION NO. : {student?.RegistrationNo ?? ""}     SYMBOL NO : {student?.RollNo ?? ""}     GRADE : {student?.Grade ?? ""}";
                AddStudentDetailRow(studentTable, regSymbolText, "", normalFont);

                // Examination year
                string examYearText = $"IN THE ANNUAL EXAMINATION CONDUCTED BY SCHOOL IN : {student?.YearBS ?? null} B.S ( {student?.YearAD ?? null} A.D ) ARE";
                AddStudentDetailRow(studentTable, examYearText, "", normalFont);

                // Given Below text
                AddStudentDetailRow(studentTable, "GIVEN BELOW.", "", normalFont);

                document.Add(studentTable);

                document.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating PDF: {ex.Message}", ex);
            }

            return stream;
        }

        private void AddStudentDetailRow(PdfPTable table, string label, string value, Font font)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;

            Paragraph p = new Paragraph();
            p.Add(new Chunk(label, font));

            if (!string.IsNullOrEmpty(value))
            {
                Phrase valuePhrase = new Phrase(value, font);
                p.Add(valuePhrase);

                // Add underlining for value fields
                PdfPTable underlineTable = new PdfPTable(1);
                underlineTable.WidthPercentage = 100;
                PdfPCell underlineCell = new PdfPCell(new Phrase("_"));
                underlineCell.Border = Rectangle.BOTTOM_BORDER;
                underlineCell.BorderWidth = 0.5f;
                underlineTable.AddCell(underlineCell);
                cell.AddElement(underlineTable);
            }

            cell.AddElement(p);
            table.AddCell(cell);
        }

        /*public MemoryStream GenerateGradeSheetPdf(List<GradeSheetSetupVM> studentDetails,List<GradeSheetSetupVM> companyInfo)
        {
            MemoryStream Stream = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 36f, 36f, 36f, 36f);
            try
            {

            PdfWriter writer = PdfWriter.GetInstance(document,Stream);
            writer.CloseStream = false;
            }catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            document.Open();

            //Add fonts
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fontPath = Path.Combine(baseDirectory,"fonts", "times.ttf");
            BaseFont baseFont = BaseFont.CreateFont(fontPath,BaseFont.IDENTITY_H,BaseFont.EMBEDDED);

            //Define fonts
            Font headerFont = new Font(baseFont, 16, Font.BOLD);
            Font normalFont = new Font(baseFont, 12, Font.NORMAL);
            Font smallFont = new Font(baseFont, 10, Font.NORMAL);
            Font tableHeaderFont = new Font(baseFont, 10,Font.BOLD);

            //Add school header
            foreach(var info in companyInfo)
            {
                // School logo (left-aligned)
                *//*if (!string.IsNullOrEmpty(info.Logo))
                {
                    Image logo = Image.GetInstance(info.Logo);
                    logo.ScaleToFit(60f, 60f);
                    logo.SetAbsolutePosition(36f, document.PageSize.Height - 100f);
                    document.Add(logo);
                }*//*

                //School name and details(center-aligned)
                Paragraph schoolName = new Paragraph(info.CompanyName,headerFont);
                schoolName.Alignment = Element.ALIGN_CENTER;
                document.Add(schoolName);

                Paragraph location = new Paragraph(info.CompanyAddress,normalFont);
                location.Alignment = Element.ALIGN_CENTER;
                document.Add(location);

                Paragraph estd = new Paragraph($"ESTD:{info.EnrollFy}", normalFont);
                estd.Alignment = Element.ALIGN_CENTER;
                document.Add(estd);
            }
            Paragraph title = new Paragraph("GRADE-SHEET",headerFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingBefore = 20f;
            title.SpacingAfter = 20f;
            document.Add(title);

            Font underlineFont = FontFactory.GetFont("HELVETICA", 12, Font.UNDERLINE);

            //Student Details
            foreach (var student in studentDetails)
            {
                PdfPTable detailsTable = new PdfPTable(1);
                detailsTable.WidthPercentage = 100;

                //First row with grade secured and student name
                PdfPCell gradeSecuredCell = new PdfPCell(new Phrase($"THE GRADE(S) SECURED BY : ", FontFactory.GetFont("HELVERICA", 12)));
                gradeSecuredCell.AddElement(new Phrase(student.SectionName, underlineFont));
                gradeSecuredCell.Border = 0;
                detailsTable.AddCell(gradeSecuredCell);
                document.Add(detailsTable);

                PdfPTable dataTable = new PdfPTable(4);
                dataTable.WidthPercentage = 100;
                dataTable.SetWidths(new float[] { 1.0f, 1.5f, 1.5f, 1.0f });

                //Add second row cells with date of birth
                dataTable.AddCell(new PdfPCell(new Phrase("DATE OF BIRTH :")) {Border =0 });
                
                //Convert DateTime to string
                PdfPCell dobCell = new PdfPCell(new Phrase(student.DOBBS,underlineFont));
                dobCell.Border = 0;
                dataTable.AddCell(dobCell);

                dataTable.AddCell(new PdfPCell(new Phrase("B.S")) { Border=0 });

                //For AD
                dataTable.AddCell(new PdfPCell(new Phrase("(A.D)")) { Border =0 });

                string dobADString = student.DOBAD.ToString("dd-MM-yyyy");
                PdfPCell dobCellAd = new PdfPCell(new Phrase(dobADString, underlineFont));
                dobCell.Border = 0;
                dataTable.AddCell(dobCellAd);
            }
            document.Close();
            return Stream;
        }*/

        /*public MemoryStream GenerateGradeSheetPdf(List<GradeSheetSetupVM> studentDetails, List<GradeSheetSetupVM> companyInfo)
        {
            if (studentDetails == null || !studentDetails.Any())
                throw new ArgumentException("Student details cannot be null or empty");

            if (companyInfo == null || !companyInfo.Any())
                throw new ArgumentException("Company information cannot be null or empty");

            MemoryStream stream = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 36f, 36f, 36f, 36f);
            PdfWriter writer = null;

            try
            {
                writer = PdfWriter.GetInstance(document, stream);
                writer.CloseStream = false;
                document.Open();

                // Add fonts - with fallback to default font if times.ttf is not found
                BaseFont baseFont;
                try
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string fontPath = Path.Combine(baseDirectory, "fonts", "times.ttf");
                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch (Exception)
                {
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                }

                Font titleFont = new Font(baseFont, 14, Font.BOLD);
                Font headerFont = new Font(baseFont, 12, Font.BOLD);
                Font normalFont = new Font(baseFont, 11, Font.NORMAL);
                Font smallFont = new Font(baseFont, 9, Font.NORMAL);

                var student = studentDetails.FirstOrDefault();
                var company = companyInfo.FirstOrDefault();

                // 1. Logo (centered at top)
               *//* if (!string.IsNullOrEmpty(company?.CompanyName))
                {
                    Image logo = Image.GetInstance(company.CompanyLogo);
                    logo.ScaleToFit(60f, 60f);
                    logo.Alignment = Element.ALIGN_CENTER;
                    document.Add(logo);
                }*//*

                // 2. School name and details
                Paragraph schoolInfo = new Paragraph();
                schoolInfo.Alignment = Element.ALIGN_CENTER;
                schoolInfo.Add(new Chunk(company?.CompanyName?.ToUpper() ?? "", headerFont));
                schoolInfo.Add(Chunk.NEWLINE);
                schoolInfo.Add(new Chunk(company?.CompanyAddress ?? "", normalFont));
                schoolInfo.Add(Chunk.NEWLINE);
                schoolInfo.Add(new Chunk($"ESTD : {company?.Estd ?? ""}", normalFont));
                schoolInfo.SpacingAfter = 10f;
                document.Add(schoolInfo);

                // 3. GRADE SHEET title (centered)
                Paragraph gradeSheetTitle = new Paragraph("GRADE SHEET", titleFont);
                gradeSheetTitle.Alignment = Element.ALIGN_CENTER;
                gradeSheetTitle.SpacingAfter = 20f;
                document.Add(gradeSheetTitle);

                // 4. Student Details Table (matches exactly with the provided design)
                PdfPTable studentTable = new PdfPTable(2);
                studentTable.WidthPercentage = 100;
                studentTable.SetWidths(new float[] { 1f, 2f });

                // The Grade(s) Secured by
                PdfPCell nameLabel = new PdfPCell(new Phrase("The Grade(s) Secured by :", normalFont));
                nameLabel.Border = Rectangle.NO_BORDER;
                PdfPCell nameValue = new PdfPCell(new Phrase(student?.StudentName ?? "", normalFont));
                nameValue.Border = Rectangle.BOTTOM_BORDER;
                studentTable.AddCell(nameLabel);
                studentTable.AddCell(nameValue);

                // Date of Birth row with B.S and A.D
                PdfPCell dobLabel = new PdfPCell(new Phrase("Date of Birth :", normalFont));
                dobLabel.Border = Rectangle.NO_BORDER;

                // Combine BS and AD dates in one cell with proper formatting
                string dobText = $"{student?.DOBBS ?? ""} B.S ({student?.DOBAD:dd-MM-yyyy} A.D)";
                PdfPCell dobValue = new PdfPCell(new Phrase(dobText, normalFont));
                dobValue.Border = Rectangle.BOTTOM_BORDER;

                studentTable.AddCell(dobLabel);
                studentTable.AddCell(dobValue);

                // Registration and Symbol Number row
                PdfPCell regLabel = new PdfPCell(
                    new Phrase($"Registration No. : {student?.RegistrationNo ?? ""} SYMBOL NO : {student?.RollNo ?? ""}", normalFont)
                );
                regLabel.Colspan = 2;
                regLabel.Border = Rectangle.NO_BORDER;
                studentTable.AddCell(regLabel);

                document.Add(studentTable);

                // 5. Examination Year Text
                Paragraph examYear = new Paragraph();
                examYear.Add(new Chunk("In the Annual Examination Conducted by School in : ", normalFont));
                examYear.Add(new Chunk($"{student?.YearBS ?? null} B.S ({student?.YearAD ?? null} A.D) are", normalFont));
                examYear.Add(new Chunk(" given below.", normalFont));
                examYear.SpacingBefore = 10f;
                examYear.SpacingAfter = 10f;
                document.Add(examYear);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating PDF: {ex.Message}", ex);
            }
            finally
            {
                if (document != null && document.IsOpen())
                {
                    document.Close();
                }
            }

            return stream;
        }

        // Helper methods
        private void AddGradeCell(PdfPTable table, string text, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, new Font(BaseFont.CreateFont(), 9)));
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.MinimumHeight = 25f;
            cell.PaddingTop = 5f;
            cell.PaddingBottom = 5f;
            table.AddCell(cell);
        }*/
        /*  private void AddDetailRow(PdfPTable table,string lable1, string value1, string lable2,string value2)
          {
              table.AddCell(new PdfPCell(new Phrase(lable1)) { Border = 0 });
              table.AddCell(new PdfPCell(new Phrase(value1)) { Border = 0 });
              table.AddCell(new PdfPCell(new Phrase(lable2)) { Border = 0 });
              table.AddCell(new PdfPCell(new Phrase(value2)) { Border = 0 });
          }
  */




    }
}