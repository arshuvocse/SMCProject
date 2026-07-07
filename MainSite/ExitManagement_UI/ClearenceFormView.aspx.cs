using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.ExitManagement_DAL;
using DAL.Permission_DAL;
using DAL.Survey;
using DAO.HRIS_DAO;
using HELPER_FUNCTIONS.HELPERS;

public partial class ExitManagement_UI_EmployeeJobLeftEntryView : System.Web.UI.Page
{

    EmployeeJobLeftEntryDAL aEmployeeJobLeftEntryDAL = new EmployeeJobLeftEntryDAL();

    ManageUserOperationCredentials aOperationCredentials = new ManageUserOperationCredentials();
    ShowMessage aShowMessage = new ShowMessage();
    Messages aMessages = new Messages();
    PermissionDAL aPermissionDal = new PermissionDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetCompany();
            //UserPersmissionValidation();

            LoadInfo();
        }
    }
    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        if ((gv.ShowHeader == true && gv.Rows.Count > 0)
            || (gv.ShowHeaderWhenEmpty == true))
        {
            //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    public void GetCompany()
    {
        DataTable dtcomp = aPermissionDal.GetCompany();
        lchk_Company.DataValueField = "CompanyId";
        lchk_Company.DataTextField = "ShortName";
        lchk_Company.DataSource = dtcomp;
        lchk_Company.DataBind();

        DataTable userdata = aPermissionDal.GetUserCompany(Session["UserId"].ToString());
        for (int i = 0; i < userdata.Rows.Count; i++)
        {
            for (int j = 0; j < lchk_Company.Items.Count; j++)
            {
                if (lchk_Company.Items[j].Text.Trim() == userdata.Rows[i]["ShortName"].ToString())
                {
                    lchk_Company.Items[j].Selected = true;


                }
            }
        }
    }

    public void UserPersmissionValidation()
    {
        try
        {
            string filepath = Path.GetDirectoryName(Request.Path);
            filepath = filepath.TrimStart('\\');
            filepath = "../" + filepath + "/" + Path.GetFileName(Request.Path);
            DataTable dtuserpermission = aPermissionDal.GetPermissionForUser(Session["UserId"].ToString(), filepath);
            if (dtuserpermission.Rows.Count > 0)
            {
                if (dtuserpermission.Rows[0]["UserTypeId"].ToString() != "3" ||
                    dtuserpermission.Rows[0]["UserTypeId"].ToString() != "4")
                {


                    ViewState["Add"] = dtuserpermission.Rows[0]["Add"].ToString();
                    ViewState["Edit"] = dtuserpermission.Rows[0]["Edit"].ToString();
                    ViewState["View"] = dtuserpermission.Rows[0]["View"].ToString();
                    ViewState["Delete"] = dtuserpermission.Rows[0]["Delete"].ToString();

                    //addNewButton.Visible = Convert.ToBoolean(ViewState["Add"].ToString());

                    loadGridView.Columns[loadGridView.Columns.Count - 1].Visible =
                        Convert.ToBoolean(ViewState["View"].ToString());
                    loadGridView.Columns[loadGridView.Columns.Count - 2].Visible =
                        Convert.ToBoolean(ViewState["Delete"].ToString());
                    loadGridView.Columns[loadGridView.Columns.Count - 3].Visible =
                        Convert.ToBoolean(ViewState["Edit"].ToString());
                }
            }
            else
            {
                Response.Redirect("../DashBoard_UI/DashBoard.aspx");
            }
        }
        catch (Exception ex)
        {

            aShowMessage.ShowMessageBox(ex.ToString(), this);
        }
    }

    public string CompanyId()
    {
        string companyid = "";
        for (int i = 0; i < lchk_Company.Items.Count; i++)
        {
            if (lchk_Company.Items[i].Selected)
            {
                companyid = companyid + "'" + lchk_Company.Items[i].Value + "'" + ",";
            }
        }
        companyid = companyid.TrimEnd(',');
        return companyid;
    }

    private void LoadInfo()
    {
        DataTable dataTable = new DataTable();

        dataTable = aEmployeeJobLeftEntryDAL.LoadInformationALlApprovalNew("   EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=0 AND EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "') ");
  //  dataTable   = aEmployeeJobLeftEntryDAL.LoadInformationALlApproval(" AND EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=0 AND EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "')");
        //DataTable dataTable = aEmployeeJobLeftEntryDAL.LoadInformationALlClear(" AND   EPE.CompanyId IN (" + CompanyId() + ") AND EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=0 AND EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "')");

        if (dataTable.Rows.Count > 0)
        {
            loadGridView.DataSource = dataTable;
            loadGridView.DataBind();


    ClearenceFormDal aEmployeeInfoListReportDAL = new ClearenceFormDal();

            int skippedIndex = -1; // Default value if no row is skipped
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                string empinfoidForMain = loadGridView.DataKeys[i]["empinfoidForMain"] != null ? loadGridView.DataKeys[i]["empinfoidForMain"].ToString() : "";
                string EmpInfoIdApproval = loadGridView.DataKeys[i]["EmpInfoIdApproval"] != null ? loadGridView.DataKeys[i]["EmpInfoIdApproval"].ToString() : "";

                if (empinfoidForMain != EmpInfoIdApproval)
                {
                    continue;
                }

                if (Session["EmpInfoId"].ToString() != "3001" && Session["DivisionId"].ToString() == "45")
                {
                //DataTable Suppervisor =
                //        aEmployeeInfoListReportDAL.GetResourceInfoforSuppervisor(
                //            Convert.ToInt32(loadGridView.DataKeys[i]["ExitMasterId"].ToString()));

                DataTable SuppervisorNullChk =
                  aEmployeeInfoListReportDAL.SuppervisorNullChkDAL(
                      Convert.ToInt32(loadGridView.DataKeys[i]["EmployeeId"].ToString()));


                HiddenField hfDivisionId = (HiddenField)loadGridView.Rows[i].Cells[0].FindControl("hfDivisionId");
                HiddenField HFExitMasterId = (HiddenField)loadGridView.Rows[i].Cells[0].FindControl("HFExitMasterId");
                HiddenField hfApprovalStatusShow = (HiddenField)loadGridView.Rows[i].Cells[0].FindControl("hfApprovalStatusShow");

                    //if (hfApprovalStatusShow.Value != null && hfApprovalStatusShow.Value == "as Supervisor")
                    //{
                    //    skippedIndex = i; // Store the skipped row index
                    //    continue; // Skip only this row
                    //}

                    if (hfDivisionId.Value != "")
                    {
                        DataTable DTpHARMA = aEmployeeJobLeftEntryDAL.ClearenceFormCHECKFORpHARMA(HFExitMasterId.Value);
                        //DataTable dataTable = aEmployeeJobLeftEntryDAL.LoadInformationALlClear(" AND   EPE.CompanyId IN (" + CompanyId() + ") AND EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=0 AND EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "')");

                        if (DTpHARMA.Rows.Count > 0)
                        {
                            string bb = DTpHARMA.Rows[0]["AppCountICT"].ToString();
                            if (bb=="1")
                            {
                                loadGridView.Rows[i].Visible = true;
                                
                            }
                            else
                            {
                                loadGridView.Rows[i].Visible = false;
                                
                            }
                             
                        }
                    }
               

                else      if (SuppervisorNullChk.Rows.Count == 0)
                {

                    loadGridView.Rows[i].Visible = true;


                }
              
                else
                {

                    //if (Suppervisor.Rows.Count > 0)
                    //{
                    //    loadGridView.Rows[i].Visible = true;
                    //}
                    //else
                    //{
                    //    loadGridView.Rows[i].Visible = false;
                    //}


                }







                }
                else if (Session["EmpInfoId"].ToString() == "3001")
                {
                    //DataTable Suppervisor =
                    //        aEmployeeInfoListReportDAL.GetResourceInfoforSuppervisor(
                    //            Convert.ToInt32(loadGridView.DataKeys[i]["ExitMasterId"].ToString()));

                    DataTable SuppervisorNullChk =
                      aEmployeeInfoListReportDAL.SuppervisorNullChkDAL(
                          Convert.ToInt32(loadGridView.DataKeys[i]["EmployeeId"].ToString()));


                    HiddenField hfDivisionId = (HiddenField)loadGridView.Rows[i].Cells[0].FindControl("hfDivisionId");
                    HiddenField HFExitMasterId = (HiddenField)loadGridView.Rows[i].Cells[0].FindControl("HFExitMasterId");
                    HiddenField hfApprovalStatusShow = (HiddenField)loadGridView.Rows[i].Cells[0].FindControl("hfApprovalStatusShow");

                    //if (hfApprovalStatusShow.Value != null && hfApprovalStatusShow.Value == "as Supervisor")
                    //{
                    //    skippedIndex = i; // Store the skipped row index
                    //    continue; // Skip only this row
                    //}

                    if (hfDivisionId.Value != "")
                    {
                        DataTable DTpHARMA = aEmployeeJobLeftEntryDAL.ClearenceFormCHECKFORpHARMA(HFExitMasterId.Value);
                        //DataTable dataTable = aEmployeeJobLeftEntryDAL.LoadInformationALlClear(" AND   EPE.CompanyId IN (" + CompanyId() + ") AND EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=0 AND EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "')");

                        if (DTpHARMA.Rows.Count > 0)
                        {
                            string bb = DTpHARMA.Rows[0]["AppCount"].ToString();
                            if (bb == "1")
                            {
                                loadGridView.Rows[i].Visible = true;

                            }
                            else
                            {
                                loadGridView.Rows[i].Visible = false;

                            }

                        }
                    }


                    else if (SuppervisorNullChk.Rows.Count == 0)
                    {

                        loadGridView.Rows[i].Visible = true;


                    }

                    else
                    {

                        //if (Suppervisor.Rows.Count > 0)
                        //{
                        //    loadGridView.Rows[i].Visible = true;
                        //}
                        //else
                        //{
                        //    loadGridView.Rows[i].Visible = false;
                        //}


                    }







                }
                
                    DataTable   dtEmpInfo =
                        aEmployeeInfoListReportDAL.GetResourceInfoApprovalPerSOn(
                            Convert.ToInt32(loadGridView.DataKeys[i]["ExitDetailId"].ToString()));

                    if (dtEmpInfo.Rows.Count>0  )
                {
                    //string name = dtEmpInfo.Rows[0].Field<string>("EmpName");
                    //LinkButton lbl = (LinkButton)loadGridView.Rows[i].Cells[0].FindControl("iinkButton");
                    //lbl.Text = "Already Checked By(" + name + ") Go To Clearance >>>";
                }

                    if (Session["EmpInfoId"].ToString() == "3001")
                {
                    loadGridView.Columns[loadGridView.Columns.Count - 2].Visible = true;
                }

                 
                    loadGridView.Columns[1].Visible = true;

               
            }
        }
    }

    protected void addNewButton_OnClick(object sender, EventArgs e)
    {
        Session["Status"] = "Add";
        Response.Redirect("EmployeeJobLeftEntry.aspx"); 
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        mpe_1.Hide();

    }
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Clearance")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);


            var datKey = loadGridView.DataKeys[rowindex];
            if (datKey != null)
            {
                string Idd = datKey[1].ToString();


                string jobLeftId = datKey[0].ToString();
                string ExitDetailId = datKey[2].ToString();
                string ApprovalStatus = datKey[3].ToString();
                string ExitMasterId = datKey[4].ToString();
                string DivisionId = datKey["DivisionId"].ToString();
                string CompanyName = datKey["CompanyName"].ToString().Trim();
                Session["EmployeeJobLeftId"] = "";
                Session["EmployeeJobLeftId"] = jobLeftId;

                //Session["Status"] = "Edit";
                string clearancePage = DivisionId != "48" &&
                                       CompanyName.Equals("SMC EL", StringComparison.OrdinalIgnoreCase)
                    ? "ClearenceFormEL.aspx"
                    : "ClearenceForm.aspx";

                Response.Redirect("~/ExitManagement_UI/" + clearancePage + "?EMPID=" + Idd + "&ExitDetailId=" + ExitDetailId + "&ApprovalStatus=" + ApprovalStatus + "&ExitMasterId=" + ExitMasterId);
            }

         

        }

        if (e.CommandName == "Clearence")
        {


            int empId = Convert.ToInt32(e.CommandArgument);

            if (empId > 0)
            {



                PopUpClearence(Convert.ToInt32(empId));



            }
        }
        if (e.CommandName == "appComm")
        {

            int rowindex = Convert.ToInt32(e.CommandArgument);


            var datKey = loadGridView.DataKeys[rowindex];
            if (datKey != null)
            {
                mpe_1.Show();
                string Idd = datKey[4].ToString();


                DataTable dtDoc = aEmployeeJobLeftEntryDAL.GetDocDataByIdOtherComment(Idd);
                if (dtDoc.Rows.Count > 0)
                {

                    gv_AppraisalFunc.DataSource = dtDoc;
                    gv_AppraisalFunc.DataBind();
                }
                else
                {
                    gv_AppraisalFunc.DataSource = null;
                    gv_AppraisalFunc.DataBind();
                }




            }



        }


        if (e.CommandName == "ViewData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);


            var datKey = loadGridView.DataKeys[rowindex];
            if (datKey != null)
            {
                string Idd = datKey[0].ToString();
                Session["EmployeeJobLeftId"] = "";
                Session["EmployeeJobLeftId"] = Idd;

                Session["Status"] = "View";

            }

            Response.Redirect("~/ExitManagement_UI/EmployeeJobLeftEntry.aspx");

        }

        if (e.CommandName == "DeleteData")
        {

            int rowindex = Convert.ToInt32(e.CommandArgument);


            var datKey = loadGridView.DataKeys[rowindex];
            if (datKey != null)
            {
                string Idd = datKey[0].ToString();
                Session["EmployeeJobLeftId"] = "";
                Session["EmployeeJobLeftId"] = Idd;
                Session["Status"] = "Delete";
            }

            Response.Redirect("~/ExitManagement_UI/EmployeeJobLeftEntry.aspx");

            //int rowindex = Convert.ToInt32(e.CommandArgument);
            //string EmployeeJobLeftId = loadGridView.DataKeys[rowindex][0].ToString();

            //if (aEmployeeJobLeftEntryDAL.DeleteEmployeeJobLeftById(EmployeeJobLeftId))
            //{
            //    LoadInfo();
            //    aShowMessage.ShowMessageBox(aMessages.DeleteMessage, this);
               
            //}
        }
    }





    protected void gv_History_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        if (e.CommandName == "Clearence")
        {
            int empId = Convert.ToInt32(e.CommandArgument);

            if (empId > 0)
            {



                PopUpClearence(Convert.ToInt32(empId));



            }
        }
       


       

    
    }

    protected void loadGridView_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        
    }

    protected void homeButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("../DashBoard_UI/DashBoard.aspx");
    }

    private void PopUpClearence(int empId)
    {
        string url = "../Report_UI/ClearenceReportViwer.aspx?rptType=" + empId;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void bllHistory_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();


        DataTable dataTable = aEmployeeJobLeftEntryDAL.LoadInformationALlApprovalsss("  and EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=1 AND  ( ( EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "')  or (EmpInfoId='" + Session["EmpInfoId"].ToString() + "') or (IsForwardEmpId='" + Session["EmpInfoId"].ToString() + "'))) ");
  //  dataTable   = aEmployeeJobLeftEntryDAL.LoadInformationALlApproval(" AND EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=0 AND EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "')");
        //DataTable dataTable = aEmployeeJobLeftEntryDAL.LoadInformationALlClear(" AND   EPE.CompanyId IN (" + CompanyId() + ") AND EIM.ExitMasterId IN (SELECT MasterId FROM dbo.tblEmpExitDetail WHERE  IsDone=0 AND EmpInfoIdApproval='" + Session["EmpInfoId"].ToString() + "')");

        if (dataTable.Rows.Count > 0)
        {
            gv_History.DataSource = dataTable;
            gv_History.DataBind();
 
        }
        else
        {
            gv_History.DataSource = null;
            gv_History.DataBind();
        }

    }

    protected void LinkButton2_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    protected string GetForwardStatusBadge(object forwardObj)
    {
        if (forwardObj == null || forwardObj == DBNull.Value) return "";
        string forward = forwardObj.ToString();
        
        string badgeStyle = "padding: 3px 8px; border-radius: 10px; font-size: 12px; font-weight: bold; display: inline-block; line-height: 1;";

        if (forward.Contains("Pending"))
        {
            return forward.Replace("Pending", "<span style=\"" + badgeStyle + " background-color: #dc3545; color: white;\">Pending</span>");
        }
        else if (forward.Contains("Done"))
        {
            return forward.Replace("Done", "<span style=\"" + badgeStyle + " background-color: #28a745; color: white;\">Done</span>");
        }
        else if (forward.Contains("N/A"))
        {
            return forward.Replace("N/A", "<span style=\"" + badgeStyle + " background-color: #ffc107; color: black;\">N/A</span>");
        }

        return forward;
    }

    protected string GetAssignListStatusColored(object statusObj)
    {
        if (statusObj == null || statusObj == DBNull.Value) return "";
        string status = statusObj.ToString();
        
        string[] parts = status.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        System.Collections.Generic.List<string> formattedParts = new System.Collections.Generic.List<string>();

        string badgeStyle = "padding: 4px 8px; border-radius: 12px; font-size: 11px; font-weight: bold; display: inline-block; line-height: 1; margin: 2px; color: white;";

        foreach (string part in parts)
        {
            if (part.Contains("| (Pending)"))
            {
                string name = part.Replace("| (Pending)", "").Trim();
                formattedParts.Add("<span style=\"" + badgeStyle + " background-color: #fd7e14;\">" + name + "</span>");
            }
            else if (part.Contains("| (Done)"))
            {
                string name = part.Replace("| (Done)", "").Trim();
                formattedParts.Add("<span style=\"" + badgeStyle + " background-color: #28a745;\">" + name + "</span>");
            }
            else
            {
                formattedParts.Add(part);
            }
        }
        
        return string.Join(" ", formattedParts);
    }
}
