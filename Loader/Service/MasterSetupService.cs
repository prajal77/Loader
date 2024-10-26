using ChannakyaCustomeDatePicker.Repository;
using Loader.DataAccess;
using Loader.EntityDataModel;
using Loader.Models;
using Loader.ViewModel.Grade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.Service
{
    public class MasterSetupService
    {
        private readonly  Repository.GenericUnitOfWork _genericUnit = null;

        public MasterSetupService()
        {
            _genericUnit = new Repository.GenericUnitOfWork();
        }

        public List<MasterGradeVM> GetGradeList()
        {
            List<MasterGradeVM> gradeList = new List<MasterGradeVM>();
            string query = "select * from FgetMasterGrade()";

            DataTable dt = CHData.GetDataTable(query);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MasterGradeVM g = new MasterGradeVM();
                    g.Mgid = Convert.ToInt32(dr["Mgid"]);
                    g.GradeName = Convert.ToString(dr["GradeName"]);
                    g.StatusId = Convert.ToInt32(dr["StatusId"]);
                    g.StatusName = Convert.ToString(dr["StatusName"]);

                    gradeList.Add(g);
                }

            }
            return gradeList;
        }


        public MasterGradeVM GetGradeById(int id)
        {
            string query = $"select * from FgetMasterGrade() where [Mgid] = {id}";
            using (SqlConnection connection = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    MasterGradeVM obj = new MasterGradeVM();
                    while (reader.Read())
                    {

                        obj.Mgid = reader.GetInt32(0);
                        obj.GradeName = reader.GetString(1);
                        obj.StatusId = reader.GetInt32(2);
                        obj.StatusName = reader.GetString(3);

                    }
                    connection.Close();

                    return obj;

                }
            }
        }


        public void SaveMasterGrade(MasterGradeVM grade)
        {
            SqlConnection con = new SqlConnection(CHData.ConnectionString);
            SqlTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                SqlCommand cmd = new SqlCommand("PcreateMasterGrade", trans.Connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;

                int MGID = (int)(cmd.Parameters.Add("@MgId", SqlDbType.Int).Value = Convert.ToInt32(grade.Mgid));

                if (MGID == 0)
                {
                    cmd.Parameters.Add("@GradeName", SqlDbType.Char).Value = grade.GradeName;
                    cmd.Parameters.Add("@StatusId", SqlDbType.Int).Value = grade.StatusId;
                    cmd.Parameters.Add("@PostedBy", SqlDbType.Int).Value = CHGlobalData.UserId;

                    //cmd.Parameters.Add("@RowId", SqlDbType.Int).Direction = ParameterDirection.InputOutput;
                    //CHGlobalData.UserId;

                    cmd.ExecuteNonQuery();

                    //grade.rowId = Convert.ToInt32(cmd.Parameters["@RowId"].Value);

                    cmd.Dispose();

                    trans.Commit();
                }

                else
                {

                    cmd.Parameters.Add("@GradeName", SqlDbType.Char).Value = grade.GradeName;
                    cmd.Parameters.Add("@StatusId", SqlDbType.Int).Value = grade.StatusId;
                    cmd.Parameters.Add("@PostedBy", SqlDbType.Int).Value = CHGlobalData.UserId;

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    trans.Commit();
                }

            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
            }
        }


        public List<SelectListItem> GetStatusList()
        {
            string query = "select distinct StatusId, StatusName from FgetMasterGrade()";
            DataTable dt = CHData.GetDataTable(query);
            List<SelectListItem> lists = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lists.Add(new SelectListItem { Value = dr["StatusId"].ToString(), Text = dr["StatusName"].ToString() });
                }
            }
            return lists;
        }

        //-------------------------------------------------------For FacultySetup -------------------------------------------------
        public List<FacultyVM> GetAllFacultyList()
        {
            string query = "select * from FgetFaculty()";
            List<FacultyVM> fac = new List<FacultyVM>();
            /* List<FacultyVM> fac = new List<FacultyVM>();

             using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
             {
                 using (SqlCommand cmd = new SqlCommand(query, con))
                 {
                     con.Open();
                     SqlDataReader reader = cmd.ExecuteReader();

                     while (reader.Read())
                     {
                         FacultyVM f = new FacultyVM
                         {
                             Fid = (int)reader["Fid"],
                             FacultyName = (string)reader["FacultyName"],
                             StatusId = (int)reader["StatusId"],
                             StatusName = (string)reader["StatusName"]
                         };
                         fac.Add(f);
                     }
                 }
             }
             return fac;*/

            fac = _genericUnit.Repository<FacultyVM>().ExecWithStoreProcedure(query).ToList();
            return fac;

        }

        public void UpsertFaculty(FacultyVM dto)
        {
            /* using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
             {
                 using (SqlCommand cmd = new SqlCommand("PCreateFaculty", con))
                 {
                     con.Open();
                     cmd.CommandType = CommandType.StoredProcedure;

                     int FID = (int)(cmd.Parameters.Add("@Fid", SqlDbType.Int).Value = Convert.ToInt32(dto.Fid));

                     if (FID == 0)
                     {
                         cmd.Parameters.Add("@FacultyName", SqlDbType.VarChar).Value = dto.FacultyName;
                         cmd.Parameters.Add("@StatusId", SqlDbType.Int).Value = dto.StatusId;
                         cmd.Parameters.Add("@PostedBy", SqlDbType.Int).Value = CHGlobalData.UserId;
                     }
                     else
                     {
                         cmd.Parameters.Add("@FacultyName", SqlDbType.VarChar).Value = dto.FacultyName;
                         cmd.Parameters.Add("@StatusId", SqlDbType.Int).Value = dto.StatusId;
                         cmd.Parameters.Add("@PostedBy", SqlDbType.Int).Value = CHGlobalData.UserId;
                     }

                     cmd.ExecuteNonQuery();
                     con.Close();

                 }
             }*/
            string query = "PcreateFaculty " + dto.Fid + ",'" + dto.FacultyName + "'," + dto.StatusId + "," + CHGlobalData.UserId;
            _genericUnit.ExecuteStoreProcedure(query);
        }

        public FacultyVM GetFacultyById(int id)
        {
            var query = $"Select * from FgetFaculty() where [Fid] = {id}";
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    FacultyVM obj = new FacultyVM();
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        obj.Fid = (int)reader["Fid"];
                        obj.FacultyName = (string)reader["FacultyName"];
                        obj.StatusId = (int)reader["StatusId"];
                        obj.StatusName = (string)reader["StatusName"];
                    }
                    con.Close();
                    return obj;
                }
            }
        }

        public List<SelectListItem> GetStatusListFaculty()
        {
            string query = "select distinct StatusId, StatusName from FgetFaculty()";
            DataTable dt = CHData.GetDataTable(query);
            List<SelectListItem> lists = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lists.Add(new SelectListItem { Value = dr["StatusId"].ToString(), Text = dr["StatusName"].ToString() });
                }
            }
            return lists;
        }

        //-------------------------------------------------------For SubjectSetup -------------------------------------------------
        public void CreateSubject(SubjectCreateVM dto)
        {
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PCreateSubjectList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmd.Parameters.Add("@ThSubCode", SqlDbType.Int).Value = dto.SubCode;
                    cmd.Parameters.Add("@ThSubjectName", SqlDbType.VarChar).Value = dto.ThSubjectName;
                    cmd.Parameters.Add("@ThFullMarks", SqlDbType.Decimal).Value = dto.ThFullMarks;
                    cmd.Parameters.Add("@ThCreditHR", SqlDbType.Decimal).Value = dto.ThCreditHR;
                    cmd.Parameters.Add("@INSubjectName", SqlDbType.VarChar).Value = dto.INSubjectName;
                    cmd.Parameters.Add("@INFullMarks", SqlDbType.Decimal).Value = dto.INFullMarks;
                    cmd.Parameters.Add("@INCreditHR", SqlDbType.Decimal).Value = dto.INCreditHR;
                    cmd.Parameters.Add("@PostedBy", SqlDbType.Int).Value = CHGlobalData.UserId;

                    cmd.ExecuteNonQuery();

                    con.Close();
                    con.Dispose();

                }
            }
        }

        public List<SubjectCreateVM> GetAllSubjectList()
        {
            string query = "Select * from dbo.FgetSubjectList() where [StatusId] = 1";
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<SubjectCreateVM> sub = new List<SubjectCreateVM>();

                    while (reader.Read())
                    {
                        SubjectCreateVM s = new SubjectCreateVM
                        {
                            Sid = (int)reader["Sid"],
                            ThSubjectCode = (string)reader["ThSubCode"],
                            ThSubjectName = (string)reader["ThSubjectName"],
                            ThFullMarks = (decimal)reader["ThFullMarks"],
                            ThCreditHR = (decimal)reader["ThCreditHR"],
                            INSubjectName = (string)reader["INSubjectName"],
                            INFullMarks = (decimal)reader["INFullMarks"],
                            INCreditHR = (decimal)reader["INCreditHR"],
                            FullMarks = (decimal)reader["FullMarks"],
                            StatusId = (int)reader["StatusId"],
                            StatusName = (string)reader["StatusName"],
                            INSubCode = (string)reader["INSubCode"]
                        };
                        sub.Add(s);

                    }
                    con.Close();
                    con.Dispose();

                    return sub;
                }

            }
        }

        //-------------------------FacultyGroup------------------------
        public List<FacultyGroupSetupVM> GetAllFacultyGroup()
        {
            string query = "select * from FgetFacultyGroupVsSubject() order by convert(int,subjectcode)";
            using(SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd= new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<FacultyGroupSetupVM> sub= new List<FacultyGroupSetupVM>();
                    while (reader.Read())
                    {
                        FacultyGroupSetupVM faculty = new FacultyGroupSetupVM 
                        {
                            FGSId = Convert.ToInt32(reader["FGSId"]),
                            FGId = Convert.ToInt32(reader["FGId"]),
                            SubId = Convert.ToInt32(reader["Subid"]),
                            SubjectType = reader["SubjectType"].ToString(),
                            Prefix = reader["Prefix"].ToString(),
                            FacultyGroupName = reader["FacultyGroupName"].ToString(),
                            SubjectCode = reader["SubjectCode"].ToString(),
                            SubjectName = reader["SubjectName"].ToString(),
                            FullMarks = Convert.ToDecimal(reader["FullMarks"]),
                            Passmarks = Convert.ToDecimal(reader["Passmarks"]),
                            Type = reader["Type"].ToString()
                        };
                        sub.Add(faculty);
                    }
                    con.Close();
                    con.Dispose();
                    return sub;
                }
            }
        }

        public void CreateFacultyGroup(FacultyGroupSetupVM dto)
        {
            string query = "PCreateFacultyGroupVsSubject"
                + dto.FGSId + "," 
                + dto.ThSubCode + ","
                + dto.SubId + ","
                + dto.FullMarks + "," 
                + dto.Passmarks + ",'" 
                + dto.SubjectCode + "','" 
                + dto.InSubjectCode + "'" 
                + dto.FullMarks +"," 
                + dto.InPassmarks + "," 
                + CHGlobalData.UserId;
            _genericUnit.ExecuteStoreProcedure(query);
        }
        public List<SelectListItem> GetFaculty()
        {
            string query = "select distinct [FGId],[FacultyGroupName] from FgetFacultyGroup() where statusId = 1";
            DataTable dt = CHData.GetDataTable(query);
            List<SelectListItem> lists = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lists.Add(new SelectListItem { Value = dr["FGId"].ToString(), Text = dr["FacultyGroupName"].ToString() });
                }
            }
            return lists;
        }
        /*public List<FacultyGroupSetupVM> GetSublist()
        {
            string query = "select distinct [ThSubCode], [ThSubjectName],[INSubCode], [INSubjectName] from FgetSubjectList()";
            List<FacultyGroupSetupVM> sublist = new List<FacultyGroupSetupVM>();
            sublist = _genericUnit.Repository<FacultyGroupSetupVM>().ExecWithStoreProcedure(query).Select(s => new FacultyGroupSetupVM
            {
                ThSubjectName = s.ThSubjectName,
                ThSubCode = s.ThSubCode,
                InSubjectCode = s.InSubjectCode,
                InSubName = s.InSubName
            }).ToList();
            return sublist;
        }*/

        public List<FacultyGroupSetupVM> GetSubjectCodes()
        {
            string query = "select [ThSubCode], [ThSubjectName], [INSubCode], [INSubjectName] From FgetSubjectList()";
            List<FacultyGroupSetupVM> lists = new List<FacultyGroupSetupVM>();

            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        FacultyGroupSetupVM f = new FacultyGroupSetupVM
                        {
                            ThSubCode = reader["ThSubCode"] != DBNull.Value ? (string)reader["ThSubCode"] : null,
                            ThSubjectName = reader["ThSubjectName"] != DBNull.Value ? (string)reader["ThSubjectName"] : null,
                            InSubjectCode = reader["InSubCode"] != DBNull.Value ? (string)reader["InSubCode"] : null,
                            INSubjectName = reader["INSubjectName"] != DBNull.Value ? (string)reader["INSubjectName"] : null

                        };
                        lists.Add(f);
                    }
                }
                con.Close();
                return lists;
            }
        }

        public List<FacultyGroupSetupVM> GetSubType()
        {
            string query = "select [SubId], [SubjectType] From SubjectType ";
            List<FacultyGroupSetupVM> list = new List<FacultyGroupSetupVM>();

            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open() ;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FacultyGroupSetupVM f = new FacultyGroupSetupVM
                        {
                            SubId = (int)reader["SubId"],
                            SubjectType= (string)reader["SubjectType"]
                        };
                        list.Add(f);
                    }
                    con.Close();
                    return list;
                }
            }
        } 
        public void CreateFaculty(FacultyGroupSetupVM dto)
        {
            string query = "PCreateFacultyGroupVsSubject @FGId, @THSubjectCode, @THSubid, @ThFullMarks, @ThPassmarks,@InSubjectCode, @InFullMarks, @InPassmarks, @PostedBy";
            var parameters = new object[]
            {
                new SqlParameter("@FGId", dto.FGId),
                new SqlParameter("@THSubjectCode", dto.ThSubCode),
                new SqlParameter("@ThFullMarks", dto.ThFullMarks),
                new SqlParameter("@THSubid", dto.SubId),
                new SqlParameter("@ThPassmarks", dto.ThPassmarks),
                new SqlParameter("@InSubjectCode", dto.InSubjectCode),
                new SqlParameter("@InFullMarks", dto.InFullMarks),
                new SqlParameter("@InPassmarks", dto.InPassmarks),
                new SqlParameter("@PostedBy", CHGlobalData.UserId)
            };
            try
            {
                int result = _genericUnit.ExecuteStoreProcedure(query, parameters);

            }catch(Exception ex)
            {
                throw new ApplicationException("Error executing stored procedure", ex);
            }
        }

        //----------------------------Class(Grade)--------------------------------

        public List<ClassSetupVM> GetClassGrade()
        {
            string query = "select * from  FgetClassGRadeExamSetup() where StatusId=1";

            using(SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<ClassSetupVM> list = new List<ClassSetupVM>();
                    while (reader.Read())
                    {
                        ClassSetupVM className = new ClassSetupVM
                        {
                            CGsId = (int)reader["CGsId"],
                            GradeName = reader["GradeName"].ToString(),
                            SectionName = reader["SectionName"].ToString(),
                            ExamName = reader["ExamName"].ToString(),
                        };
                        list .Add(className);
                    }
                    con.Close();
                    con.Dispose();
                    return list;
                }
            }   
        }

       public List<ClassSetupVM> GetGrade()
        {
            string query = "select [Mgid],[GradeName] from FgetMasterGrade() where StatusId=1";

            List<ClassSetupVM> list = new List<ClassSetupVM> ();

            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ClassSetupVM f = new ClassSetupVM()
                        {
                            Mgid = (int)reader["Mgid"],
                            GradeName= (string)reader["GradeName"]
                        };
                        list .Add(f);
                    }
                    con.Close();
                    con.Dispose();
                    return list;
                }
            }
        }
     public List<ClassSetupVM> GetSectionLists(int id)
        {
            string query = $"select SecId,SectionName from FgetSectionList() where (StatusId = 1 and [ClassId] = {id} )";
            List<ClassSetupVM> lists = _genericUnit
                                .Repository<ClassSetupVM>().
                                ExecWithStoreProcedure(query)
                                .Select(x=> new ClassSetupVM
                                {
                                    SectionId = x.SecId,
                                    SectionName = x.SectionName

                                }).ToList();
            return lists;
        }
        
        public List<ClassSetupVM> GetExamTerm()
        {
            string query = "select [ETermId],[TermName] from MasterExamTerm";
            List<ClassSetupVM> list = new List<ClassSetupVM>();
            using (SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        ClassSetupVM f = new ClassSetupVM()
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

        public ClassSetupVM GetClassById(int id)
        {
            var query = $"Select * from FgetClassGRadeExamSetup() where CGsId = {id}";

            using(SqlConnection con = new SqlConnection(CHData.ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    ClassSetupVM obj = new ClassSetupVM();
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        obj.CGsId=(int)reader["CGsId"];
                        obj.GradeId = (int)reader["GradeId"];
                        obj.SecId = (int)reader["SectionId"];
                        obj.ETermId = (int)reader["ETermId"];
                        obj.StatusId = (int)reader["StatusId"];
                        obj.ExamName = (string)reader["ExamName"];
                    }
                    con.Close();
                    return obj;
                }
            }
        }
        
        public void UpsertClass(ClassSetupVM dto)
        {
            string query = "PcreateClassGRadeExamSetup @CGsId,@GradeId,@SectionId,@ETermId,@StatusId,@PostedBy";
            var parameters = new object[]
            {
                new SqlParameter("@CGsId",dto.CGsId),
                new SqlParameter("@GradeId",dto.GradeId),
                new SqlParameter("@SectionId",dto.SectionId),
                new SqlParameter("@ETermId",dto.ETermId),
                new SqlParameter("@StatusId",dto.StatusId),
                new SqlParameter("@PostedBy",CHGlobalData.UserId)
            };
            int result = _genericUnit.ExecuteStoreProcedure(query, parameters);
        }
        
    } 
}

