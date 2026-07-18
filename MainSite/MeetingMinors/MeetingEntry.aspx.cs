using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.COMMON_DAL;
using DAL.MeetingMinorsDAL;
using DAO.HRIS_DAO;
using DAO.HRIS_DAO_EF;
using DAO.MeetingMinorsDAO;
using HELPER_FUNCTIONS.HELPERS;

public partial class MeetingMinors_MeetingEntry : System.Web.UI.Page
{
    MiscellaneousInformationDAL AMAsterDal = new MiscellaneousInformationDAL();
    MeetingEntryDAL AMeetingEntryDal = new MeetingEntryDAL();
    private SubcommitteeSetupDAL AMAster = new SubcommitteeSetupDAL();
    MemberInfoDaL aMinors = new MemberInfoDaL();
    
    // Session-backed caches so dropdown data survives across postbacks
    // and is not re-fetched on every add/remove click.
    private DataTable CompaniesCache
    {
        get
        {
            if (Session["__MtgEntry_Companies"] == null)
                Session["__MtgEntry_Companies"] = aMinors.GetAllCompaniesForRadioButton();
            return (DataTable)Session["__MtgEntry_Companies"];
        }
    }

    private DataTable GetEmployeesForCompany(string companyId)
    {
        string key = "__MtgEntry_Emps_" + companyId;
        if (Session[key] == null)
            Session[key] = AMAsterDal.GetDDLEmpInfo(companyId);
        return (DataTable)Session[key];
    }

    // Member position options (Chairman, Member, etc.) — static reference data,
    // no need to re-query the DB on every add/remove click.
    private DataTable MemberPositionCache
    {
        get
        {
            if (Session["__MtgEntry_MemberPos"] == null)
                Session["__MtgEntry_MemberPos"] = aMinors.GetDDLMemberPostion();
            return (DataTable)Session["__MtgEntry_MemberPos"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
         Page.Form.Attributes.Add("enctype", "multipart/form-data");
         FUDocument.Attributes["onchange"] = "UploadFile(this)";
        if (!IsPostBack)
        {
       ButtonVisible();
            LoadInitialGrid();
            LoadInitialGridDetails_Save();
            LoadDropDownList();

            if (!string.IsNullOrEmpty(Request.QueryString["MID"]))
            {

                id_mastetID.Value = (Request.QueryString["MID"]);

                DataTable dtMaster = AMeetingEntryDal.GetMasterDataById(id_mastetID.Value);
                if (dtMaster.Rows.Count > 0)
                {

                    ddlCompany.SelectedValue = dtMaster.Rows[0]["CompanyId"].ToString();
                    
                    ddlCompany_OnSelectedIndexChanged(null, null);
                    txtTitle.Text = dtMaster.Rows[0]["Title"].ToString();
                    ddlCategory.SelectedValue = dtMaster.Rows[0]["CategoryID"].ToString();
                    ddlCategory_OnSelectedIndexChanged(null, null);
                    ddlSubCommittee.SelectedValue = dtMaster.Rows[0]["SubCommitteeId"].ToString();

                    txtMeetingpurpose.Text = dtMaster.Rows[0]["MeetingPurpose"].ToString();
                    //ddlClassification.SelectedValue = dtMaster.Rows[0]["Classification"].ToString();

                    try
                    {
                        txtMeetingDate.Text = Convert.ToDateTime(dtMaster.Rows[0]["MeetingDate"]).ToString("dd-MMM-yyyy");
                    }
                    catch (Exception)
                    {
                        
                        //throw;
                    }

                    txtStartTime.Text = dtMaster.Rows[0]["StartTime"].ToString();
                    txtEndTime.Text = dtMaster.Rows[0]["EndTime"].ToString();


                    try
                    {

                      bool  selectrb =Convert.ToBoolean(dtMaster.Rows[0]["IsNotice"].ToString());

                        if (selectrb)
                        {
                            rbNotice.Items[0].Selected = true;
                        }
                        else
                        {
                            rbNotice.Items[1].Selected = true;
                        }
                       
                            
                     
                    }
                    catch (Exception)
                    {

                        //throw;
                    }

           bool     IsOfficePremisis=     Convert.ToBoolean(dtMaster.Rows[0]["IsOfficePremisis"].ToString());
                       bool     IsOuterPremisis=        Convert.ToBoolean(dtMaster.Rows[0]["IsOuterPremisis"].ToString());
                       bool     IsVirtualMeeting=        Convert.ToBoolean(dtMaster.Rows[0]["IsVirtualMeeting"].ToString());

                    if (IsOfficePremisis==true)
                    {
                        rbLocation.Items[0].Selected = true;

                      



                    }
                    if (IsOuterPremisis == true)
                    {
                        rbLocation.Items[1].Selected = true;
                    }
                    if (IsVirtualMeeting == true)
                    {
                        rbLocation.Items[2].Selected = true;
                    }

                    rbLocation_OnSelectedIndexChanged(null, null);



                    if (IsOfficePremisis == true)
                    {
                      

                        ddlOffice.SelectedValue = dtMaster.Rows[0]["OfficeId"].ToString();
                        ddlOffice_OnSelectedIndexChanged(null, null);
                        ddlLocation.SelectedValue = dtMaster.Rows[0]["LocationId"].ToString();
                        ddlLocation_OnSelectedIndexChanged(null, null);
                        ddlFloor.SelectedValue = dtMaster.Rows[0]["FloorId"].ToString();
                        ddlFloor_OnSelectedIndexChanged(null, null);

                        ddlMettingRoomName.SelectedValue = dtMaster.Rows[0]["MettingRoomId"].ToString();
                        ddlMettingRoomName_OnSelectedIndexChanged(null, null);


                    }
                    if (IsOuterPremisis == true)
                    {
                        rbLocation.Items[1].Selected = true;

                        txtLocation.Text = dtMaster.Rows[0]["Location"].ToString();
                        txtDescription.Text = dtMaster.Rows[0]["LocationDescription"].ToString();



                    }
                    if (IsVirtualMeeting == true)
                    {
                        rbLocation.Items[2].Selected = true;
                        txtRemarks.Text = dtMaster.Rows[0]["Remarks"].ToString();

                    }




                }

                DataTable dtDoc = AMeetingEntryDal.GetDocDataById(id_mastetID.Value);
                if (dtDoc.Rows.Count > 0)
                {
                    ViewState["DocGrid_List"] = dtDoc;
                    gv_DocumentUpload.DataSource = dtDoc;
                    gv_DocumentUpload.DataBind();
                }

                DataTable MeetingInfoDetail = AMeetingEntryDal.GetMeetingInfoDetailByIdEmp(id_mastetID.Value);
                if (MeetingInfoDetail.Rows.Count > 0)
                {
                    ViewState["gv_Details_List"] = MeetingInfoDetail;
                    gv_Details_Save.DataSource = MeetingInfoDetail;
                    gv_Details_Save.DataBind();


                    for (int i = 0; i < MeetingInfoDetail.Rows.Count; i++)
                    {
                        RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType"));
                        rbType.SelectedValue = MeetingInfoDetail.Rows[i]["Type"].ToString();

                        CheckBoxList chkNotification = ((CheckBoxList)gv_Details_Save.Rows[i].FindControl("chkNotification"));

                        bool NotificationEmail = Convert.ToBoolean(MeetingInfoDetail.Rows[i]["NotificationEmail"].ToString());
                        bool NotificationSMS = Convert.ToBoolean(MeetingInfoDetail.Rows[i]["NotificationSMS"].ToString());

                        if (NotificationEmail==true)
                        {
                            chkNotification.Items[0].Selected = true;
                        }

                        if (NotificationSMS == true)
                        {
                            chkNotification.Items[1].Selected = true;
                        }


 


                        RadioButtonList chkPosition = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("chkPosition"));



                        chkPosition.SelectedValue = MeetingInfoDetail.Rows[i]["Position"].ToString();

                    }
                }


                DataTable MeetingInfoDetailemp = AMeetingEntryDal.GetMeetingInfoDetailByIdBoardMember(id_mastetID.Value);
                if (MeetingInfoDetailemp.Rows.Count > 0)
                {
                    ViewState["gv_BoardMember_List"] = MeetingInfoDetailemp;
                    gv_BoardMember.DataSource = MeetingInfoDetailemp;
                    gv_BoardMember.DataBind();

                    DataTable dtMemberPostion = aMinors.GetDDLMemberPostion();

                    for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
                    {
                        DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
                        ddlPosition.DataSource = dtMemberPostion;
                        ddlPosition.DataValueField = "Value";
                        ddlPosition.DataTextField = "TextField";
                        ddlPosition.DataBind();

                       
                                ddlPosition.SelectedItem.Text = MeetingInfoDetailemp.Rows[i]["Position"].ToString();
                           


                        
                    }
                    //for (int i = 0; i < MeetingInfoDetailemp.Rows.Count; i++)
                    //{



                    //    DropDownList ddlPosition = ((DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition"));

                    //    RadioButtonList chkBoardMemberPosition = ((RadioButtonList)gv_BoardMember.Rows[i].FindControl("chkBoardMemberPosition"));



                    //    chkBoardMemberPosition.SelectedValue = MeetingInfoDetailemp.Rows[i]["Position"].ToString();
                    //    ddlPosition.SelectedValue = MeetingInfoDetailemp.Rows[i]["Position"].ToString();

                    //}
                }
                DataTable ApprovalPath = AMeetingEntryDal.GetMeetingApprovalPathById(id_mastetID.Value);
                if (ApprovalPath.Rows.Count > 0)
                {
                    ViewState["gv_Details_App"] = ApprovalPath;
                    gv_ApprovalPathDetail.DataSource = ApprovalPath;
                    gv_ApprovalPathDetail.DataBind();



                    for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                    {
                        DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("ddlSequenceList");


                        ddlSequenceList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select One..", String.Empty));
                        for (int k = 1; k < gv_ApprovalPathDetail.Rows.Count + 1; k++)
                        {
                            ddlSequenceList.Items.Insert(k, new ListItem(k.ToString()));
                        }
                    }


                    for (int i = 0; i < ApprovalPath.Rows.Count; i++)
                    {
                        DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("ddlSequenceList");
                        HiddenField hfSeq_No = (HiddenField)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("hfSeq_No");

                        ddlSequenceList.SelectedValue = hfSeq_No.Value;


                        HiddenField hfIsMinimumApprovalPerson = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfIsMinimumApprovalPerson"));
                        HiddenField hfCanEdit = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfCanEdit"));
                        HiddenField HiNotificationEmailApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("HiNotificationEmailApp"));
                        HiddenField hfNotificationSMSApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfNotificationSMSApp"));


                        CheckBox chkMimimumCount = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkMimimumCount");

                        if (hfIsMinimumApprovalPerson.Value != "")
                        {
                            chkMimimumCount.Checked = Convert.ToBoolean(hfIsMinimumApprovalPerson.Value);

                        }


                        CheckBox chkIsEdit = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkIsEdit");

                        if (hfCanEdit.Value != "")
                        {
                            chkIsEdit.Checked = Convert.ToBoolean(hfCanEdit.Value);

                        }
                        CheckBoxList chkNotificationApp = (CheckBoxList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkNotificationApp");


                        if (HiNotificationEmailApp.Value != "")
                        {
                            chkNotificationApp.Items[0].Selected = Convert.ToBoolean(HiNotificationEmailApp.Value);

                        }

                        if (hfNotificationSMSApp.Value != "")
                        {
                            chkNotificationApp.Items[1].Selected = Convert.ToBoolean(hfNotificationSMSApp.Value);

                        }


                    }
                    lblstatus.Text = "";


                }
                else
                {
                    lblstatus.Text = "No Approval Path have been  selected.";

                }
                DataTable dtAgendaa = AMeetingEntryDal.GetAgendDataById(id_mastetID.Value);
                if (dtAgendaa.Rows.Count > 0)
                {
                   
                    gv_AgendaList.DataSource = dtAgendaa;
                    gv_AgendaList.DataBind();


                    for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
                    {


                        HiddenField hfImplementationStatus = ((HiddenField)gv_AgendaList.Rows[i].FindControl("hfImplementationStatus"));
                        DropDownList ddlImplementationStatus = ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlImplementationStatus"));
                        ddlImplementationStatus.SelectedValue = hfImplementationStatus.Value;

                    }
                }


            }
        }
    }
    private void LoadInitialGrid()
    {
        DataTable dtDetails = new DataTable();

        dtDetails.Columns.Add("Agenda");
        dtDetails.Columns.Add("Remarks");

        dtDetails.Columns.Add("Presentor");
        dtDetails.Columns.Add("Observation");
        dtDetails.Columns.Add("Decision");
        dtDetails.Columns.Add("ImplementationStatus");


        DataRow dr = null;
        dr = dtDetails.NewRow();

        dr["Agenda"] = "";

        dr["Presentor"] = "";
        dr["Remarks"] = "";
        dr["Observation"] = "";
        dr["Decision"] = "";
        dr["ImplementationStatus"] = "";
 

        dtDetails.Rows.Add(dr);

        gv_AgendaList.DataSource = null;
        gv_AgendaList.DataBind();
        gv_AgendaList.DataSource = dtDetails;
        gv_AgendaList.DataBind();

    }


    private void LoadInitialGridDetails_Save()
    {
        DataTable dtDetails = new DataTable();

        dtDetails.Columns.Add("Type");
        dtDetails.Columns.Add("CompanyId");

        dtDetails.Columns.Add("EmpMasterCode");
        dtDetails.Columns.Add("EmpInfoId");
        dtDetails.Columns.Add("EmpName");
        dtDetails.Columns.Add("Designation");
        dtDetails.Columns.Add("NotificationEmail");
        dtDetails.Columns.Add("NotificationSMS");
        dtDetails.Columns.Add("Position");
        dtDetails.Columns.Add("IsBoardMember");
        dtDetails.Columns.Add("BMemberSetupDetailsID");
        
        

        DataRow dr = null;
        dr = dtDetails.NewRow();

        dr["Type"] = "";
        dr["CompanyId"] = "";

        dr["EmpMasterCode"] = "";
        dr["EmpInfoId"] = "";
        dr["EmpName"] = "";
        dr["Designation"] = "";

        dr["NotificationEmail"] = "";
        dr["NotificationSMS"] = "";
        dr["Position"] = "";
        dr["IsBoardMember"] = "";
        dr["BMemberSetupDetailsID"] = "";
        



        dtDetails.Rows.Add(dr);

        gv_Details_Save.DataSource = null;
        gv_Details_Save.DataBind();
        gv_Details_Save.DataSource = dtDetails;
        gv_Details_Save.DataBind();

    }

    private void LoadInitialGridBoardMember()
    {
        DataTable dtDetails = new DataTable();
         
         
        dtDetails.Columns.Add("EmpName");
        dtDetails.Columns.Add("Designation"); 
        dtDetails.Columns.Add("Position");
        dtDetails.Columns.Add("BMemberSetupDetailsID"); 

        

        DataRow dr = null;
        dr = dtDetails.NewRow();

        
        dr["EmpName"] = "";
        dr["Designation"] = "";


        dr["Position"] = "";
        dr["BMemberSetupDetailsID"] = "";
       



        dtDetails.Rows.Add(dr);

        gv_BoardMember.DataSource = null;
        gv_BoardMember.DataBind();
        gv_BoardMember.DataSource = dtDetails;
        gv_BoardMember.DataBind();

    }

    public void ButtonVisible()
    {
        if (Session["Status"] != null)
        {
            if (Session["Status"].ToString() == "Add")
            {
                submitButton.Visible = true;
                //lbDraft.Visible = true;
            }
            else if (Session["Status"].ToString() == "Edit")
            {
                editButton.Visible = true;

                //lbDraft.Visible = true;

            }
            else if (Session["Status"].ToString() == "Delete")
            {
                delButton.Visible = true;
            }
            Session["Status"] = null;
        }
        else
        {

            Response.Redirect("MeetingStatusList.aspx");
        }


    }

    private CommonDataLoadDAL _commonDataLoad = new CommonDataLoadDAL();

    private void LoadDropDownList()
    {
        AMAsterDal.GetCompanyListIntoDropdown(ddlCompany);
        AMAsterDal.GetCompanyListIntoDropdown(ddlCompanyLocation);
        AMAsterDal.GetCategoryListIntoDropdown(ddlCategory);
        AMAsterDal.GetCompanyListIntoDropdownAll(ddlComSearch);
        ddlCompany.SelectedIndex = 1;
        ddlComSearch.SelectedIndex = 1;

        //using (DataTable dt = _commonDataLoad.GetDDLSalaryLocation())
        //{
        //    ddlOffice.DataSource = dt;
        //    ddlOffice.DataValueField = "Value";
        //    ddlOffice.DataTextField = "TextField";
        //    ddlOffice.DataBind();
        //}

        ddlCompany_OnSelectedIndexChanged(null, null);
        ddlComSearch_OnSelectedIndexChanged(null, null);

        MemberInfoDaL aMinors = new MemberInfoDaL();


         DataTable dtss = aMinors.loadMember("");

         if (dtss.Rows.Count > 0)
        {
            gv_loadGridView.DataSource = dtss;
            gv_loadGridView.DataBind();
        }
         else
         {
             gv_loadGridView.DataSource = null;
             gv_loadGridView.DataBind();
             
         }

    }
    protected void ddlDivision_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDivision.SelectedValue != "")
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModal2').modal('show')", true);

            AMAsterDal.GetDepartmentByDivList(ddlDepartment, ddlDivision.SelectedValue);

        }
        else
        {

            ddlDepartment.Items.Clear();
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
    private bool docVali()
    {
        if (hfDocFile.Value == "")
        {
            aShowMessage.ShowMessageBox("Please Upload a Document", this);

            return false;
        }
        if (txtSummaryNote.Text == "")
        {
            aShowMessage.ShowMessageBox("Please Enter Summary Note ", this);

            return false;
        }
        return true;

    }

    protected void brnAddDoc_OnClick(object sender, EventArgs e)
    {
        if (docVali())
        {
            AddNewDocGrid_List();
        }
       
    }
    private void AddNewDocGrid_List()
    {
        string extractedText = ExtractUploadedDocumentText();

        if (ViewState["DocGrid_List"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["DocGrid_List"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {
                drCurrentRow = dtCurrentTable.NewRow();


                drCurrentRow["DocumentLink"] = "../UploadMeetingDocument/" + hfDocFile.Value;
                drCurrentRow["FileName"] = hfDocFileName.Value;
                drCurrentRow["ExtractedText"] = extractedText;




                drCurrentRow["DocumentNote"] = txtSummaryNote.Text.Trim();

                dtCurrentTable.Rows.Add(drCurrentRow);
                //Store the current data to ViewState for future reference   
                ViewState["DocGrid_List"] = dtCurrentTable;

                //Rebind the Grid with the current data to reflect changes   
                gv_DocumentUpload.DataSource = dtCurrentTable;
                gv_DocumentUpload.DataBind();
            }
        }
        else
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("DocumentLink", typeof(string)));
            dt.Columns.Add(new DataColumn("DocumentNote", typeof(string)));
            dt.Columns.Add(new DataColumn("FileName", typeof(string)));
            dt.Columns.Add(new DataColumn("ExtractedText", typeof(string)));


            dr = dt.NewRow();


            dr["DocumentLink"] = "../UploadMeetingDocument/" + hfDocFile.Value;
            dr["FileName"] = hfDocFileName.Value;
            dr["ExtractedText"] = extractedText;




            dr["DocumentNote"] = txtSummaryNote.Text.Trim();
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState for future reference   
            ViewState["DocGrid_List"] = dt;

            //Bind the Gridview   
            gv_DocumentUpload.DataSource = dt;
            gv_DocumentUpload.DataBind();
        }
        //Set Previous Data on Postbacks   
        SetDocGrid_List();


        txtSummaryNote.Text = string.Empty;
       // HyperLink2.Text = "No File Uploaded";
        HyperLink2.NavigateUrl = "";
        hfDocFile.Value = "";
    }

    private string ExtractUploadedDocumentText()
    {
        try
        {
            string filePath = Server.MapPath("~/UploadMeetingDocument/" + Path.GetFileName(hfDocFile.Value));
            string tessDataPath = Server.MapPath("~/App_Data/tessdata");
            return AttachmentOcrService.ExtractText(filePath, tessDataPath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError("Meeting attachment OCR failed: " + ex);
            return string.Empty;
        }
    }
    protected void btnDocRemove_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["DocGrid_List"] != null)
        {
            DataTable dt = (DataTable)ViewState["DocGrid_List"];
            dt.Rows.Remove(dt.Rows[rowID]);
            if (dt.Rows.Count > 0)
            {
                //Store the current data in ViewState for future reference  
                ViewState["DocGrid_List"] = dt;
                //Re bind the GridView for the updated data  
                gv_DocumentUpload.DataSource = dt;
                gv_DocumentUpload.DataBind();
            }
            else
            {
                ViewState["DocGrid_List"] = null;
                //Re bind the GridView for the updated data  
                gv_DocumentUpload.DataSource = null;
                gv_DocumentUpload.DataBind();
            }
        }
        //Set Previous Data on Postbacks  
        SetDocGrid_List();
    }
    private void SetDocGrid_List()
    {
        int rowIndex = 0;
        if (ViewState["DocGrid_List"] != null)
        {
            DataTable dt = (DataTable)ViewState["DocGrid_List"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HiddenField hfDocumentLink = (HiddenField)gv_DocumentUpload.Rows[rowIndex].FindControl("hfDocumentLink");
                    HyperLink HLDocumentLink = (HyperLink)gv_DocumentUpload.Rows[rowIndex].FindControl("HLDocumentLink");
                    Label lbl_DocumentLink = (Label)gv_DocumentUpload.Rows[rowIndex].FindControl("lbl_DocumentLink");

                    Label lbl_DocumentNote = (Label)gv_DocumentUpload.Rows[rowIndex].FindControl("lbl_DocumentNote");
                    HiddenField hfFileName = (HiddenField)gv_DocumentUpload.Rows[rowIndex].FindControl("hfFileName");


                    if (i < dt.Rows.Count - 1)
                    {
                        hfFileName.Value = dt.Rows[i]["FileName"].ToString();

                        hfDocumentLink.Value = dt.Rows[i]["DocumentLink"].ToString();
                        lbl_DocumentLink.Text = dt.Rows[i]["DocumentLink"].ToString();
                        HLDocumentLink.NavigateUrl = dt.Rows[i]["DocumentLink"].ToString();

                        lbl_DocumentNote.Text = dt.Rows[i]["DocumentNote"].ToString();

                    }

                    rowIndex++;
                }
            }
        }
    }

    protected void btnDocUp_OnClick(object sender, EventArgs e)
    {
        if (FUDocument.HasFile)
        {
            string _fileExt = System.IO.Path.GetExtension(FUDocument.FileName);
            string AdsFile = "Meeting_Mis_DOC_" + Guid.NewGuid().ToString() + Path.GetExtension(FUDocument.FileName);
            //  fileName = guid.ToString() + imageFileUpload.FileName;
            FUDocument.SaveAs(Server.MapPath("../UploadImg/") + AdsFile);
            HyperLink2.NavigateUrl = "../UploadImg/" + AdsFile;
            HyperLink2.Text = "Uploaded Successfully";
        }
        else
        {
            HyperLink2.NavigateUrl = "";
        //    HyperLink2.Text = "No File Uploaded";
        }
    }
    protected void btn_DetailsRemove_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["gv_Details_List"] != null)
        {
            DataTable dt = (DataTable)ViewState["gv_Details_List"];
            for (int i = 0; i < gv_Details_Save.Rows.Count; i++)
            {
                RadioButtonList rbType = (RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType");
                RadioButtonList ddlCompanySave = (RadioButtonList)gv_Details_Save.Rows[i].FindControl("ddlCompanySave");
                DropDownList ddlEmployeeSave = (DropDownList)gv_Details_Save.Rows[i].FindControl("ddlEmployeeSave");
                TextBox txt_EmpName = (TextBox)gv_Details_Save.Rows[i].FindControl("txt_EmpName");
                TextBox txt_Designation = (TextBox)gv_Details_Save.Rows[i].FindControl("txt_Designation");
                TextBox txt_EmpMasterCode = (TextBox)gv_Details_Save.Rows[i].FindControl("txt_EmpMasterCode");
                RadioButtonList chkPosition = (RadioButtonList)gv_Details_Save.Rows[i].FindControl("chkPosition");
                CheckBoxList chkNotification = (CheckBoxList)gv_Details_Save.Rows[i].FindControl("chkNotification");
                
                try { dt.Rows[i]["Type"] = rbType != null ? rbType.SelectedValue : ""; } catch {}
                try { dt.Rows[i]["EmpName"] = txt_EmpName != null ? txt_EmpName.Text : ""; } catch {}
                try { dt.Rows[i]["Designation"] = txt_Designation != null ? txt_Designation.Text : ""; } catch {}
                try { dt.Rows[i]["EmpInfoId"] = ddlEmployeeSave != null ? ddlEmployeeSave.SelectedValue : ""; } catch {}
                try { dt.Rows[i]["EmpMasterCode"] = txt_EmpMasterCode != null ? txt_EmpMasterCode.Text : ""; } catch {}
                try { dt.Rows[i]["CompanyId"] = ddlCompanySave != null ? ddlCompanySave.SelectedValue : ""; } catch {}
                try { dt.Rows[i]["Position"] = chkPosition != null ? chkPosition.SelectedValue : ""; } catch {}
                try 
                { 
                    if (chkNotification != null && chkNotification.Items.Count > 1)
                    {
                        dt.Rows[i]["NotificationEmail"] = chkNotification.Items[0].Selected.ToString();
                        dt.Rows[i]["NotificationSMS"] = chkNotification.Items[1].Selected.ToString();
                    }
                } 
                catch {}
                
                HiddenField hfCompanySave = (HiddenField)gv_Details_Save.Rows[i].FindControl("hfCompanySave");
                if (hfCompanySave != null && ddlCompanySave != null) hfCompanySave.Value = ddlCompanySave.SelectedValue;
            }

            dt.Rows.Remove(dt.Rows[rowID]);
            if (dt.Rows.Count > 0)
            {
                ViewState["gv_Details_List"] = dt;
                gv_Details_Save.DataSource = dt;
                gv_Details_Save.DataBind();
            }
            else
            {
                ViewState["gv_Details_List"] = null;
                gv_Details_Save.DataSource = null;
                gv_Details_Save.DataBind();
            }
        }
        //Set Previous Data on Postbacks  
        //  SetDocGrid_List();


        for (int i = 0; i < gv_Details_Save.Rows.Count; i++)
        {
            RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType"));
            rbType.SelectedValue = ((HiddenField)gv_Details_Save.Rows[i].Cells[1].FindControl("hfType"))
                    .Value;



            CheckBoxList chkNotification = ((CheckBoxList)gv_Details_Save.Rows[i].FindControl("chkNotification"));

            HiddenField HiNotificationEmail = ((HiddenField)gv_Details_Save.Rows[i].FindControl("HiNotificationEmail"));
            HiddenField hfNotificationSMS = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfNotificationSMS"));

            if (HiNotificationEmail.Value != "")
            {
                try
                {
                    chkNotification.Items[0].Selected = Convert.ToBoolean(HiNotificationEmail.Value);
                }
                catch (Exception)
                {

                    //throw;
                }

            }

            if (hfNotificationSMS.Value != "")
            {
                try
                {
                    chkNotification.Items[1].Selected = Convert.ToBoolean(hfNotificationSMS.Value);
                }
                catch (Exception)
                {

                    //throw;
                }

            }


            RadioButtonList chkPosition = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("chkPosition"));

            HiddenField hfPosition = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfPosition"));

            chkPosition.SelectedValue = hfPosition.Value;

        }
    }

    private bool AddMemberValidation()
    {
        if (gv_EmpListSearch.Rows.Count == 0)
        {

            AlertMessageBoxShow("Please select at least one employee !!!");
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModal2').modal('show')", true);

            return false;
        }

        int totalCount = gv_EmpListSearch.Rows.Cast<GridViewRow>().Count(r => ((CheckBox)r.FindControl("chkSelect")).Checked);

        if (totalCount == 0)
        {
            aShowMessage.ShowMessageBox("Please Select Employee", this);
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModal2').modal('show')", true);



            return false;
        }






        return true;
    }


    private bool AddMemberValidationApp()
    {
        if (gv_EmpListSearchAPP.Rows.Count == 0)
        {

            AlertMessageBoxShow("Please select at least one employee !!!");
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalAPPEmpp').modal('show')", true);

            return false;
        }

        int totalCount = gv_EmpListSearchAPP.Rows.Cast<GridViewRow>().Count(r => ((CheckBox)r.FindControl("chkSelectAPP")).Checked);

        if (totalCount == 0)
        {
            aShowMessage.ShowMessageBox("Please Select Employee", this);
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalAPPEmpp').modal('show')", true);



            return false;
        }






        return true;
    }



    private bool BordmemberMemberValidationApp()
    {
        if (gv_loadGridView.Rows.Count == 0)
        {

            AlertMessageBoxShow("Please select at least one employee !!!");
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalBoardMember').modal('show')", true);

            return false;
        }

        int totalCount = gv_loadGridView.Rows.Cast<GridViewRow>().Count(r => ((CheckBox)r.FindControl("BchkSelect")).Checked);

        if (totalCount == 0)
        {
            aShowMessage.ShowMessageBox("Please Select Employee", this);
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalBoardMember').modal('show')", true);



            return false;
        }






        return true;
    }


    public bool CheckBoardMemberApp()
    {
        for (int i = 0; i < gv_loadGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)gv_loadGridView.Rows[i].Cells[0].FindControl("BchkSelect");
            for (int j = 0; j < gv_ApprovalPathDetail.Rows.Count; j++)
            {
                if (chkBoxRows.Checked)
                {
                    Label SSStxt_empId = (Label)gv_ApprovalPathDetail.Rows[j].FindControl("Applbl_EmpName");

                    Label EmpDI = (Label)gv_loadGridView.Rows[i].FindControl("BM_Name");

                    if (EmpDI.Text == SSStxt_empId.Text)
                    {


                        return false;

                    }

                }

            }

        }
        return true;
    }
    public bool CheckEmpListApp()
    {
        for (int i = 0; i < gv_EmpListSearchAPP.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)gv_EmpListSearchAPP.Rows[i].Cells[0].FindControl("chkSelectAPP");
            for (int j = 0; j < gv_ApprovalPathDetail.Rows.Count; j++)
            {
                if (chkBoxRows.Checked)
                {
                    Label SSStxt_empId = (Label)gv_ApprovalPathDetail.Rows[j].FindControl("Appbl_EmpMasterCode");

                    Label EmpDI = (Label)gv_EmpListSearchAPP.Rows[i].FindControl("lbl_EmpMasterCodeAPP");

                    if (EmpDI.Text == SSStxt_empId.Text)
                    {


                        return false;

                    }

                }

            }

        }
        return true;
    }
    public bool CheckEmpList()
    {
        for (int i = 0; i < gv_EmpListSearch.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)gv_EmpListSearch.Rows[i].Cells[0].FindControl("chkSelect");
            for (int j = 0; j < gv_Details_Save.Rows.Count; j++)
            {
                if (chkBoxRows.Checked)
                {
                    TextBox SSStxt_empId = (TextBox)gv_Details_Save.Rows[j].FindControl("txt_EmpMasterCode");

                    Label EmpDI = (Label)gv_EmpListSearch.Rows[i].FindControl("lbl_EmpMasterCode");

                    if (EmpDI.Text == SSStxt_empId.Text)
                    {


                        return false;

                    }

                }

            }

        }
        return true;
    }

    private DataTable CreateDraftMemberTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("EmpInfoId");
        table.Columns.Add("EmpMasterCode");
        table.Columns.Add("EmpName");
        table.Columns.Add("Designation");
        return table;
    }

    private void BindDraftMemberList(DataTable table)
    {
        ViewState["DraftMemberList"] = table;
        gv_DraftMemberList.DataSource = table;
        gv_DraftMemberList.DataBind();
    }

    protected void btnStageMembers_OnClick(object sender, EventArgs e)
    {
        if (!AddMemberValidation())
        {
            return;
        }

        DataTable draft = ViewState["DraftMemberList"] as DataTable ?? CreateDraftMemberTable();
        var draftCodes = new HashSet<string>(draft.AsEnumerable()
            .Select(row => row["EmpMasterCode"].ToString()), StringComparer.OrdinalIgnoreCase);
        var savedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (GridViewRow row in gv_Details_Save.Rows)
        {
            TextBox code = row.FindControl("txt_EmpMasterCode") as TextBox;
            if (code != null && !string.IsNullOrWhiteSpace(code.Text))
            {
                savedCodes.Add(code.Text.Trim());
            }
        }

        foreach (GridViewRow row in gv_EmpListSearch.Rows)
        {
            CheckBox selected = row.FindControl("chkSelect") as CheckBox;
            if (selected == null || !selected.Checked)
            {
                continue;
            }

            HiddenField empInfoId = row.FindControl("hfEmpInfoId") as HiddenField;
            Label employeeCode = row.FindControl("lbl_EmpMasterCode") as Label;
            Label employeeName = row.FindControl("lbl_EmpName") as Label;
            Label designation = row.FindControl("lbl_Designation") as Label;
            string code = employeeCode == null ? string.Empty : employeeCode.Text.Trim();

            if (string.IsNullOrWhiteSpace(code) || draftCodes.Contains(code) || savedCodes.Contains(code))
            {
                continue;
            }

            DataRow draftRow = draft.NewRow();
            draftRow["EmpInfoId"] = empInfoId == null ? string.Empty : empInfoId.Value;
            draftRow["EmpMasterCode"] = code;
            draftRow["EmpName"] = employeeName == null ? string.Empty : employeeName.Text;
            draftRow["Designation"] = designation == null ? string.Empty : designation.Text;
            draft.Rows.Add(draftRow);
            draftCodes.Add(code);
        }

        BindDraftMemberList(draft);
        ClientScript.RegisterStartupScript(GetType(), "Popup", "$('#exampleModal2').modal('show')", true);
    }

    protected void btnRemoveDraftMember_OnClick(object sender, EventArgs e)
    {
        LinkButton button = (LinkButton)sender;
        GridViewRow gridRow = (GridViewRow)button.NamingContainer;
        DataTable draft = ViewState["DraftMemberList"] as DataTable;

        if (draft != null && gridRow.RowIndex >= 0 && gridRow.RowIndex < draft.Rows.Count)
        {
            string removedCode = draft.Rows[gridRow.RowIndex]["EmpMasterCode"].ToString();
            draft.Rows.RemoveAt(gridRow.RowIndex);

            foreach (GridViewRow searchRow in gv_EmpListSearch.Rows)
            {
                Label code = searchRow.FindControl("lbl_EmpMasterCode") as Label;
                CheckBox selected = searchRow.FindControl("chkSelect") as CheckBox;
                if (code != null && selected != null &&
                    string.Equals(code.Text.Trim(), removedCode, StringComparison.OrdinalIgnoreCase))
                {
                    selected.Checked = false;
                }
            }

            BindDraftMemberList(draft);
        }

        ClientScript.RegisterStartupScript(GetType(), "Popup", "$('#exampleModal2').modal('show')", true);
    }

    protected void btnSubmitDraftMembers_OnClick(object sender, EventArgs e)
    {
        DataTable draft = ViewState["DraftMemberList"] as DataTable;
        if (draft == null || draft.Rows.Count == 0)
        {
            AlertMessageBoxShow("Please add at least one employee to the draft list.");
            ClientScript.RegisterStartupScript(GetType(), "Popup", "$('#exampleModal2').modal('show')", true);
            return;
        }

        gv_EmpListSearch.DataSource = draft;
        gv_EmpListSearch.DataBind();
        foreach (GridViewRow row in gv_EmpListSearch.Rows)
        {
            CheckBox selected = row.FindControl("chkSelect") as CheckBox;
            if (selected != null)
            {
                selected.Checked = true;
            }
        }

        btnAddToListEmp_OnClick(sender, e);
        ViewState["DraftMemberList"] = null;
        gv_DraftMemberList.DataSource = null;
        gv_DraftMemberList.DataBind();
    }

    protected void btnAddToListEmp_OnClick(object sender, EventArgs e)
    {
        if (AddMemberValidation())
        {
            if (CheckEmpList())
            {

                DataTable aDataTable = new DataTable();
                aDataTable.Columns.Add("EmpInfoId");
                aDataTable.Columns.Add("EmpMasterCode");
                aDataTable.Columns.Add("EmpName");
                aDataTable.Columns.Add("Designation");


                aDataTable.Columns.Add("Type");
                aDataTable.Columns.Add("NotificationEmail");
                aDataTable.Columns.Add("NotificationSMS");
                aDataTable.Columns.Add("Position");
                aDataTable.Columns.Add("IsBoardMember");
                aDataTable.Columns.Add("BMemberSetupDetailsID");




                DataRow dataRow = null;

        int count = 0;

                for (int i = 0; i < gv_EmpListSearch.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)gv_EmpListSearch.Rows[i].Cells[0].FindControl("chkSelect");
                    HiddenField hfEmpInfoId = ((HiddenField)gv_EmpListSearch.Rows[i].FindControl("hfEmpInfoId"));
                    Label lbl_EmpMasterCode = (Label)gv_EmpListSearch.Rows[i].FindControl("lbl_EmpMasterCode");
                    Label lbl_EmpName = (Label)gv_EmpListSearch.Rows[i].FindControl("lbl_EmpName");
                    Label lbl_Designation = (Label)gv_EmpListSearch.Rows[i].FindControl("lbl_Designation");

                    if (chkSelect.Checked)
                    {
                        count++;
                        //  if (HasDCStoreId(Convert.ToInt32(loadGridView.DataKeys[i][0].ToString())))
                        {



                            dataRow = aDataTable.NewRow();
                            dataRow["IsBoardMember"] = "0";
                            dataRow["BMemberSetupDetailsID"] = "0";
                            dataRow["EmpInfoId"] = hfEmpInfoId.Value;

                            dataRow["EmpMasterCode"] = lbl_EmpMasterCode.Text;
                            dataRow["EmpName"] = lbl_EmpName.Text;
                            dataRow["Designation"] = lbl_Designation.Text;

                            dataRow["Type"] = "";

                            dataRow["NotificationEmail"] = "";
                            dataRow["NotificationSMS"] = "";
                          

                            dataRow["Position"] = "";

                            dataRow["IsBoardMember"] = "0";

                            aDataTable.Rows.Add(dataRow);
                        }
                    }
                }
                for (int i = 0; i < gv_Details_Save.Rows.Count; i++)
                {


                    HiddenField hfIsBoardMember = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfIsBoardMember"));
                    HiddenField hfBMemberSetupDetailsID = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfBMemberSetupDetailsID"));
                    HiddenField hfType = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfType"));
                    TextBox txt_EmpMasterCode = ((TextBox)gv_Details_Save.Rows[i].FindControl("txt_EmpMasterCode"));
                    HiddenField ShfEmpInfoId = ((HiddenField)gv_Details_Save.Rows[i].FindControl("ShfEmpInfoId"));
                    TextBox txt_EmpName = (TextBox)gv_Details_Save.Rows[i].FindControl("txt_EmpName");
                    TextBox txt_Designation = (TextBox)gv_Details_Save.Rows[i].FindControl("txt_Designation");

                    HiddenField HiNotificationEmail = ((HiddenField)gv_Details_Save.Rows[i].FindControl("HiNotificationEmail"));
                    HiddenField hfNotificationSMS = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfNotificationSMS"));
                    HiddenField hfPosition = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfPosition"));



                    RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType"));
                    CheckBoxList chkNotification = ((CheckBoxList)gv_Details_Save.Rows[i].FindControl("chkNotification"));
                    RadioButtonList chkPosition = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("chkPosition"));
                   


             



                    dataRow = aDataTable.NewRow();
                    dataRow["EmpInfoId"] = ShfEmpInfoId.Value;
                    dataRow["IsBoardMember"] = hfIsBoardMember.Value;
                    dataRow["BMemberSetupDetailsID"] = hfBMemberSetupDetailsID.Value;

                    dataRow["EmpMasterCode"] = txt_EmpMasterCode.Text;
                    dataRow["EmpName"] = txt_EmpName.Text;
                    dataRow["Designation"] = txt_Designation.Text;
                    
                    dataRow["Type"] = hfType.Value.Trim();
                    if (hfType.Value.Trim()!="")
                    {
                        try
                        {
                            rbType.SelectedValue = hfType.Value.Trim();
                        }
                        catch (Exception)
                        {
                            
                            //throw;
                        }
                         
                    }
                    dataRow["NotificationEmail"] = HiNotificationEmail.Value.Trim();
                    if (HiNotificationEmail.Value.Trim() != "")
                    {
                        try
                        {
                            chkNotification.Items[0].Selected = Convert.ToBoolean(HiNotificationEmail.Value.Trim());
                        }
                        catch (Exception)
                        {
                            chkNotification.Items[0].Selected = false;
                            // throw;
                        }

                    }

                    dataRow["NotificationSMS"] = hfNotificationSMS.Value.Trim();
                    if (hfNotificationSMS.Value.Trim() != "")
                    {
                        try
                        {
                            chkNotification.Items[1].Selected = Convert.ToBoolean(hfNotificationSMS.Value.Trim());
                        }
                        catch (Exception)
                        {

                            chkNotification.Items[1].Selected = false;
                        }

                    }

                    dataRow["Position"] = hfPosition.Value.Trim();
                    if (hfPosition.Value.Trim()!="")
                    {
                        try
                        {
                            chkPosition.SelectedValue = hfPosition.Value.Trim();
                        }
                        catch (Exception)
                        {
                            
                            //throw;
                        }
                    } 
                   







                    aDataTable.Rows.Add(dataRow);
                }
                ViewState["gv_Details_List"] = aDataTable;
                gv_Details_Save.DataSource = aDataTable;
                gv_Details_Save.DataBind();



                 


                for (int i = 0; i < gv_Details_Save.Rows.Count; i++)
                {
                    RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType"));
                    rbType.SelectedValue = ((HiddenField)gv_Details_Save.Rows[i].Cells[1].FindControl("hfType"))
                            .Value;



                    CheckBoxList chkNotification = ((CheckBoxList)gv_Details_Save.Rows[i].FindControl("chkNotification"));

                    HiddenField HiNotificationEmail = ((HiddenField)gv_Details_Save.Rows[i].FindControl("HiNotificationEmail"));
                    HiddenField hfNotificationSMS = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfNotificationSMS"));

                    if (HiNotificationEmail.Value != "")
                    {
                        try
                        {
                            chkNotification.Items[0].Selected = Convert.ToBoolean(HiNotificationEmail.Value);
                        }
                        catch (Exception)
                        {

                            //throw;
                        }

                    }

                    if (hfNotificationSMS.Value != "")
                    {
                        try
                        {
                            chkNotification.Items[1].Selected = Convert.ToBoolean(hfNotificationSMS.Value);
                        }
                        catch (Exception)
                        {

                            //throw;
                        }

                    }


                    RadioButtonList chkPosition = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("chkPosition"));

                    HiddenField hfPosition = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfPosition"));

                    chkPosition.SelectedValue = hfPosition.Value;

                }


                for (int i = 0; i < gv_EmpListSearch.Rows.Count; i++)
                {
                    var chkBoxRows = (CheckBox)gv_EmpListSearch.Rows[i].Cells[0].FindControl("chkSelect");
                    for (int j = 0; j < gv_Details_Save.Rows.Count; j++)
                    {
                        if (chkBoxRows.Checked)
                        {
                            TextBox SSStxt_empId = (TextBox)gv_Details_Save.Rows[j].FindControl("txt_EmpMasterCode");

                            Label EmpDI = (Label)gv_EmpListSearch.Rows[i].FindControl("lbl_EmpMasterCode");

                            if (EmpDI.Text == SSStxt_empId.Text)
                            {

                                RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[j].FindControl("rbType"));
                                rbType.SelectedValue = "Employee";

                                ((HiddenField)gv_Details_Save.Rows[j].Cells[1].FindControl("hfType"))
                                    .Value = "Employee";
                               

                            }

                        }

                    }

                }
            }
            else
            {
                AlertMessageBoxShow("Already Exist !!!");
            }
        }

    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var chkBoxHeader = (CheckBox)gv_EmpListSearch.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < gv_EmpListSearch.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)gv_EmpListSearch.Rows[i].Cells[0].FindControl("chkSelect");
            chkBoxRows.Checked = chkBoxHeader.Checked;
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModal2').modal('show')", true);

    }

    ShowMessage aShowMessage = new ShowMessage();

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModal2').modal('show')", true);
        if (ddlCompany.SelectedIndex > 0)
        {
            LoadEMPInfo();
        }
        else
        {
            ddlCompany.Focus();
            aShowMessage.ShowMessageBox("Please Select This !!!", this);
        }
    }

    private string GenerateParameter()
    {
        string parameter = " ";

        if (ddlComSearch.SelectedIndex > 0)
        {
            parameter = parameter + "  and    com.CompanyId = '" + ddlComSearch.SelectedValue + "'";
        }

        if (ddlDivision.SelectedIndex > 0)
        {
            parameter = parameter + "  and    div.DivisionId = '" + ddlDivision.SelectedValue + "'";
        }

        if (ddlDepartment.SelectedIndex > 0)
        {
            parameter = parameter + "  and    Dpt.DepartmentId = '" + ddlDepartment.SelectedValue + "'";
        }

        if (ddlEmp.SelectedIndex > 0)
        {
            parameter = parameter + "  and   e.EmpInfoId = '" + ddlEmp.SelectedValue + "'";
        }
        return parameter;
    }

    private string GenerateParameterApp()
    {
        string parameter = " ";

        if (ddlCompany.SelectedIndex > 0)
        {
            parameter = parameter + "  and    com.CompanyId = '" + ddlCompany.SelectedValue + "'";
        }

        if (ddlDivisionAPP.SelectedIndex > 0)
        {
            parameter = parameter + "  and    div.DivisionId = '" + ddlDivisionAPP.SelectedValue + "'";
        }

        if (ddlDepartmentAPP.SelectedIndex > 0)
        {
            parameter = parameter + "  and    Dpt.DepartmentId = '" + ddlDepartmentAPP.SelectedValue + "'";
        }
        return parameter;
    }

    private void LoadEMPInfo()
    {
        DataTable jobCreationInfos = new DataTable();
        jobCreationInfos = AMAsterDal.GetEMpInfos(GenerateParameter());
        if (jobCreationInfos.Rows.Count > 0)
        {

            gv_EmpListSearch.DataSource = jobCreationInfos;
            gv_EmpListSearch.DataBind();



        }
        else
        {
            gv_EmpListSearch.DataSource = null;
            gv_EmpListSearch.DataBind();
            AlertMessageBoxShow("No Data Found!!!");

        }
    }


    private void LoadEMPInfoAappEmp()
    {
        DataTable jobCreationInfos = new DataTable();
        jobCreationInfos = AMAsterDal.GetEMpInfos(GenerateParameterApp());
        if (jobCreationInfos.Rows.Count > 0)
        {

            gv_EmpListSearchAPP.DataSource = jobCreationInfos;
            gv_EmpListSearchAPP.DataBind();



        }
        else
        {
            gv_EmpListSearchAPP.DataSource = null;
            gv_EmpListSearchAPP.DataBind();
            AlertMessageBoxShow("No Data Found!!!");

        }
    }
    private void AlertMessageBoxShow(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

    }

    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue != "")
        {
            using (DataTable dt = AMAsterDal.GetDDLComDivision(ddlCompany.SelectedValue))
            {
                 

                ddlDivisionAPP.DataSource = dt;
                ddlDivisionAPP.DataValueField = "Value";
                ddlDivisionAPP.DataTextField = "TextField";
                ddlDivisionAPP.DataBind();
            }
            if (Session["UserTypeId"].ToString() == "3" ||
               Session["UserTypeId"].ToString() == "4")
            {
                using (DataTable dt2 = AMAsterDal.GetDDLRoutingPath(ddlCompany.SelectedValue))
                {
                    rbRoutingPath.DataSource = dt2;
                    rbRoutingPath.DataValueField = "Value";
                    rbRoutingPath.DataTextField = "TextField";
                    rbRoutingPath.DataBind();
                }
            }
            else
            {
                using (DataTable dt2 = AMAsterDal.GetDDLRoutingPathDepartment(ddlCompany.SelectedValue, Session["DepartmentId"].ToString()))
                {
                    rbRoutingPath.DataSource = dt2;
                    rbRoutingPath.DataValueField = "Value";
                    rbRoutingPath.DataTextField = "TextField";
                    rbRoutingPath.DataBind();
                }
            }

            //    using (DataTable dt2 = AMeetingEntryDal.GetEmpInfobyID(ddlCompany.SelectedValue))
            //    {

            //        for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
            //        {
            //DropDownList ddlPresentor= ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlPresentor"));

            //ddlPresentor.DataSource = dt2;
            //ddlPresentor.DataValueField = "EmpInfoId";
            //ddlPresentor.DataTextField = "EmpName";
            //ddlPresentor.DataBind();
            //ddlPresentor.Items.Insert(0, new ListItem("Please Select an Employee.....", String.Empty));
            //ddlPresentor.SelectedIndex = 0;
            //        }
            //    }

            // Reload member list and subcommittees based on the new company selection
            //ddlCategory_OnSelectedIndexChanged(null, null);
            gv_BoardMember.DataSource = null;
            gv_BoardMember.DataBind();
            DataTable jobCreationInfos = new DataTable();
            jobCreationInfos = AMeetingEntryDal.GetEmpMemberInfoByCategory(ddlCompany.SelectedValue);
            if (jobCreationInfos.Rows.Count > 0)
            {
                ViewState["gv_BoardMember_List"] = jobCreationInfos;
                gv_BoardMember.DataSource = jobCreationInfos;
                gv_BoardMember.DataBind();
                DataTable dtMemberPostion = MemberPositionCache;

                for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
                {
                    DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
                    ddlPosition.DataSource = dtMemberPostion;
                    ddlPosition.DataValueField = "Value";
                    ddlPosition.DataTextField = "TextField";
                    ddlPosition.DataBind();

                    // Fix: use row index i, not inner loop over all rows (was O(n²))
                    try
                    {
                        if (i < jobCreationInfos.Rows.Count)
                            ddlPosition.SelectedValue = jobCreationInfos.Rows[i]["PositionId"].ToString();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        else
        {
            //for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
            //{
            //    DropDownList ddlPresentor = ((DropDownList) gv_AgendaList.Rows[i].Cells[2].FindControl("ddlPresentor"));
            //    ddlPresentor.Items.Clear();
            //}
           
             rbRoutingPath.Items.Clear();
        }
    }

    protected void btnAgenaListAdd_OnClick(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;

        DataTable aTable = new DataTable();
        aTable.Columns.Add("Agenda");
        aTable.Columns.Add("Presentor");
        aTable.Columns.Add("Remarks");
        aTable.Columns.Add("Observation");
        aTable.Columns.Add("Decision");
        aTable.Columns.Add("ImplementationStatus");


        DataRow dr;



        for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
        {
            dr = aTable.NewRow();
            dr["Agenda"] =
                   ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtAgenda")).Text.Trim();


            dr["Remarks"] =
                ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtRemarks")).Text.Trim();

            dr["Presentor"] =
                ((HiddenField)gv_AgendaList.Rows[i].Cells[2].FindControl("hfPresentor")).Value.Trim();


          
            dr["Presentor"] =
             ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlPresentor")).SelectedValue.Trim();


            dr["Observation"] =
              ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtObservation")).Text.Trim();
            dr["Decision"] =
               ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtDecision")).Text.Trim();


            dr["ImplementationStatus"] =
               ((HiddenField)gv_AgendaList.Rows[i].Cells[2].FindControl("hfImplementationStatus")).Value.Trim();



            dr["ImplementationStatus"] =
             ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlImplementationStatus")).SelectedValue.Trim();


            aTable.Rows.Add(dr);

            if (rowIndex == i)
            {
                dr = aTable.NewRow();
                dr["Agenda"] = "";
                dr["Presentor"] = "";
                dr["Remarks"] = "";
                dr["Observation"] = "";
                dr["Decision"] = "";
                dr["ImplementationStatus"] = "";

                aTable.Rows.Add(dr);
            }
        }

        //Session["table"] = (DataTable)aTable;
        gv_AgendaList.DataSource = null;
        gv_AgendaList.DataBind();
        gv_AgendaList.DataSource = aTable;
        gv_AgendaList.DataBind();

        //using (DataTable dt2 = AMeetingEntryDal.GetEmpInfobyID(ddlCompany.SelectedValue))
        //{

        //    for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
        //    {
        //        DropDownList ddlPresentor = ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlPresentor"));

        //        ddlPresentor.DataSource = dt2;
        //        ddlPresentor.DataValueField = "EmpInfoId";
        //        ddlPresentor.DataTextField = "EmpName";
        //        ddlPresentor.DataBind();
        //        ddlPresentor.Items.Insert(0, new ListItem("Please Select an Employee.....", String.Empty));
        //        ddlPresentor.SelectedIndex = 0;



        //        ddlPresentor.SelectedValue = ((HiddenField)gv_AgendaList.Rows[i].Cells[1].FindControl("hfPresentor"))
        //                 .Value;
        //    }
        //}

        for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
        {


            HiddenField hfImplementationStatus = ((HiddenField)gv_AgendaList.Rows[i].FindControl("hfImplementationStatus"));
            DropDownList ddlImplementationStatus = ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlImplementationStatus"));
            ddlImplementationStatus.SelectedValue = hfImplementationStatus.Value;
           
        }

    }

    protected void btnAgenaLisRemove_OnClick(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;

        DataTable aTable = new DataTable();
        aTable.Columns.Add("Agenda");
        aTable.Columns.Add("Presentor");
        aTable.Columns.Add("Remarks");
        aTable.Columns.Add("Observation");
        aTable.Columns.Add("Decision");
        aTable.Columns.Add("ImplementationStatus");


        DataRow dr;

        for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
        {
            if (rowIndex != i)
            {
                dr = aTable.NewRow();
                dr["Remarks"] =
               ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtRemarks")).Text.Trim();
                dr["Agenda"] =
                  ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtAgenda")).Text.Trim();

                dr["Presentor"] =
                    ((HiddenField)gv_AgendaList.Rows[i].Cells[2].FindControl("hfPresentor")).Value.Trim();
                dr["Presentor"] =
           ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlPresentor")).SelectedValue.Trim();

                dr["Observation"] =
        ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtObservation")).Text.Trim();
                dr["Decision"] =
                   ((TextBox)gv_AgendaList.Rows[i].Cells[2].FindControl("txtDecision")).Text.Trim();
                dr["ImplementationStatus"] =
           ((HiddenField)gv_AgendaList.Rows[i].Cells[2].FindControl("hfImplementationStatus")).Value.Trim();



                dr["ImplementationStatus"] =
                 ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlImplementationStatus")).SelectedValue.Trim();


                aTable.Rows.Add(dr);
            }
        }

        if (aTable.Rows.Count > 0)
        {
            gv_AgendaList.DataSource = null;
            gv_AgendaList.DataBind();
            gv_AgendaList.DataSource = aTable;
            gv_AgendaList.DataBind();



        }

        for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
        {


            HiddenField hfImplementationStatus = ((HiddenField)gv_AgendaList.Rows[i].FindControl("hfImplementationStatus"));
            DropDownList ddlImplementationStatus = ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlImplementationStatus"));
            ddlImplementationStatus.SelectedValue = hfImplementationStatus.Value;

        }
        //using (DataTable dt2 = AMeetingEntryDal.GetEmpInfobyID(ddlCompany.SelectedValue))
        //{

        //    for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
        //    {
        //        DropDownList ddlPresentor = ((DropDownList)gv_AgendaList.Rows[i].Cells[2].FindControl("ddlPresentor"));

        //        ddlPresentor.DataSource = dt2;
        //        ddlPresentor.DataValueField = "EmpInfoId";
        //        ddlPresentor.DataTextField = "EmpName";
        //        ddlPresentor.DataBind();
        //        ddlPresentor.Items.Insert(0, new ListItem("Please Select an Employee.....", String.Empty));
        //        ddlPresentor.SelectedIndex = 0;



        //        ddlPresentor.SelectedValue = ((HiddenField)gv_AgendaList.Rows[i].Cells[1].FindControl("hfPresentor"))
        //                 .Value;
        //    }
        //}
    }

    protected void btnOkayRoutingPath_OnClick(object sender, EventArgs e)
    {
        string grade = "";
        for (int i = 0; i < rbRoutingPath.Items.Count; i++)
        {
            if (rbRoutingPath.Items[i].Selected)
            {
                grade = rbRoutingPath.Items[i].Value;
            }
        }

        if (grade == "")
        {
            AlertMessageBoxShow("Select Routing Path!!!");
        }
        else
        {

            DataTable jobCreationInfos = new DataTable();
            jobCreationInfos = AMAsterDal.GetEMpInfoFromRoutingPath(grade);
            if (jobCreationInfos.Rows.Count > 0)
            {
                lblstatus.Text = "";
                ViewState["gv_Details_App"] = jobCreationInfos;
                gv_ApprovalPathDetail.DataSource = jobCreationInfos;
                gv_ApprovalPathDetail.DataBind();


            



                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {
                    DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("ddlSequenceList");


                    ddlSequenceList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select One..", String.Empty));
                    for (int k = 1; k < gv_ApprovalPathDetail.Rows.Count + 1; k++)
                    {
                        ddlSequenceList.Items.Insert(k, new ListItem(k.ToString()));
                    }
                }


                for (int i = 0; i < jobCreationInfos.Rows.Count; i++)
                {
                    DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("ddlSequenceList");
                    HiddenField hfSeq_No = (HiddenField)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("hfSeq_No");

                    ddlSequenceList.SelectedValue = hfSeq_No.Value;

                }


            }
            else
            {
                gv_ApprovalPathDetail.DataSource = null;
                gv_ApprovalPathDetail.DataBind();
                lblstatus.Text = "No Approval Path have been  selected.";


            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#exampleModal').modal('hide')", true);

        }




    }

    protected void Appbtn_DetailsRemove_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["gv_Details_App"] != null)
        {
            DataTable dt = (DataTable)ViewState["gv_Details_App"];
            dt.Rows.Remove(dt.Rows[rowID]);
            if (dt.Rows.Count > 0)
            {
                //Store the current data in ViewState for future reference  
                ViewState["gv_Details_App"] = dt;
                //Re bind the GridView for the updated data  
                gv_ApprovalPathDetail.DataSource = dt;
                gv_ApprovalPathDetail.DataBind();
            }
            else
            {
                ViewState["gv_Details_App"] = null;
                //Re bind the GridView for the updated data  
                gv_ApprovalPathDetail.DataSource = null;
                gv_ApprovalPathDetail.DataBind();
            }


            for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
            {
                DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].FindControl("ddlSequenceList");


                ddlSequenceList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select One.....", String.Empty));
                for (int k = 1; k < gv_ApprovalPathDetail.Rows.Count + 1; k++)
                {
                    ddlSequenceList.Items.Insert(k, new ListItem(k.ToString()));
                }
            }

            for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
            {
                DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("ddlSequenceList");
                HiddenField hfSeq_No = (HiddenField)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("hfSeq_No");

                ddlSequenceList.SelectedValue = hfSeq_No.Value;




                HiddenField hfIsMinimumApprovalPerson = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfIsMinimumApprovalPerson"));
                HiddenField hfCanEdit = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfCanEdit"));
                HiddenField HiNotificationEmailApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("HiNotificationEmailApp"));
                HiddenField hfNotificationSMSApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfNotificationSMSApp"));


                CheckBox chkMimimumCount = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkMimimumCount");

                if (hfIsMinimumApprovalPerson.Value != "")
                {
                    chkMimimumCount.Checked = true;

                }


                CheckBox chkIsEdit = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkIsEdit");

                if (hfCanEdit.Value != "")
                {
                    chkIsEdit.Checked = true;

                }
                CheckBoxList chkNotificationApp = (CheckBoxList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkNotificationApp");


                if (HiNotificationEmailApp.Value != "")
                {
                    chkNotificationApp.Items[0].Selected = true;

                }

                if (hfNotificationSMSApp.Value != "")
                {
                    chkNotificationApp.Items[1].Selected = true;

                }

            }
        }


        if (gv_ApprovalPathDetail.Rows.Count == 0)
        {
            lblstatus.Text = "No Approval Path have been  selected.";
        }
        else
        {
            lblstatus.Text = "";
        }
    }


    protected void ddlDivisionAPP_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDivisionAPP.SelectedValue != "")
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalAPPEmpp').modal('show')", true);

            AMAsterDal.GetDepartmentByDivList(ddlDepartmentAPP, ddlDivisionAPP.SelectedValue);

        }
        else
        {

            ddlDepartmentAPP.Items.Clear();
        }
    }

    protected void btnSearchAPP_OnClick(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalAPPEmpp').modal('show')", true);
        if (ddlCompany.SelectedIndex > 0)
        {
            LoadEMPInfoAappEmp();
        }
        else
        {
            ddlCompany.Focus();
            aShowMessage.ShowMessageBox("Please Select This !!!", this);
        }
    }

    protected void chkSelectAllAPP_CheckedChanged(object sender, EventArgs e)
    {
        var chkBoxHeader = (CheckBox)gv_EmpListSearchAPP.HeaderRow.FindControl("chkSelectAllAPP");

        for (int i = 0; i < gv_EmpListSearchAPP.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)gv_EmpListSearchAPP.Rows[i].Cells[0].FindControl("chkSelectAPP");
            chkBoxRows.Checked = chkBoxHeader.Checked;
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalAPPEmpp').modal('show')", true);
    }

    protected void btnAddToListEmpAPP_OnClick(object sender, EventArgs e)
    {
        if (AddMemberValidationApp())
        {
            if (CheckEmpListApp())
            {

                DataTable aDataTable = new DataTable();
                aDataTable.Columns.Add("EmpInfoId");
                aDataTable.Columns.Add("EmpMasterCode");
                aDataTable.Columns.Add("EmpName");
                aDataTable.Columns.Add("Designation");
                aDataTable.Columns.Add("Seq_No");
                aDataTable.Columns.Add("IsMinimumApprovalPerson");
                aDataTable.Columns.Add("CanEdit");
                aDataTable.Columns.Add("NotificationEmail");
                aDataTable.Columns.Add("NotificationSMS");
               



                DataRow dataRow = null;

                for (int i = 0; i < gv_EmpListSearchAPP.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)gv_EmpListSearchAPP.Rows[i].Cells[0].FindControl("chkSelectAPP");
                    HiddenField hfEmpInfoId = ((HiddenField)gv_EmpListSearchAPP.Rows[i].FindControl("hfEmpInfoIdAPP"));
                    Label lbl_EmpMasterCode = (Label)gv_EmpListSearchAPP.Rows[i].FindControl("lbl_EmpMasterCodeAPP");
                    Label lbl_EmpName = (Label)gv_EmpListSearchAPP.Rows[i].FindControl("lbl_EmpNameAPP");
                    Label lbl_Designation = (Label)gv_EmpListSearchAPP.Rows[i].FindControl("lbl_DesignationAPP");

                    if (chkSelect.Checked)
                    {
                        //  if (HasDCStoreId(Convert.ToInt32(loadGridView.DataKeys[i][0].ToString())))
                        {



                            dataRow = aDataTable.NewRow();

                            dataRow["EmpInfoId"] = hfEmpInfoId.Value;

                            dataRow["EmpMasterCode"] = lbl_EmpMasterCode.Text;
                            dataRow["EmpName"] = lbl_EmpName.Text;
                            dataRow["Designation"] = lbl_Designation.Text;

                            dataRow["Seq_No"] = "";
                            dataRow["IsMinimumApprovalPerson"] = "";
                            dataRow["CanEdit"] = "";
                            dataRow["NotificationEmail"] = "";
                            dataRow["NotificationSMS"] = "";
                          

                            aDataTable.Rows.Add(dataRow);
                        }
                    }
                }
                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {


                    HiddenField ShfEmpInfoId = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("ApphfEmpInfoId"));
                    HiddenField hfSeq_No = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfSeq_No"));
                    Label Slbl_EmpMasterCode = (Label)gv_ApprovalPathDetail.Rows[i].FindControl("Appbl_EmpMasterCode");
                    Label Slbl_EmpName = (Label)gv_ApprovalPathDetail.Rows[i].FindControl("Applbl_EmpName");
                    Label Slbl_Designation = (Label)gv_ApprovalPathDetail.Rows[i].FindControl("Applbl_Designation");



                    HiddenField hfIsMinimumApprovalPerson = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfIsMinimumApprovalPerson"));
                    HiddenField hfCanEdit = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfCanEdit"));
                    HiddenField HiNotificationEmailApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("HiNotificationEmailApp"));
                    HiddenField hfNotificationSMSApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfNotificationSMSApp"));



                    dataRow = aDataTable.NewRow();
                    dataRow["EmpInfoId"] = ShfEmpInfoId.Value;

                    dataRow["EmpMasterCode"] = Slbl_EmpMasterCode.Text;
                    dataRow["EmpName"] = Slbl_EmpName.Text;
                    dataRow["Designation"] = Slbl_Designation.Text;
                    dataRow["Seq_No"] = hfSeq_No.Value;


                    dataRow["IsMinimumApprovalPerson"] = hfIsMinimumApprovalPerson.Value;
                    dataRow["CanEdit"] = hfCanEdit.Value;
                    dataRow["NotificationEmail"] = HiNotificationEmailApp.Value;
                    dataRow["NotificationSMS"] = hfNotificationSMSApp.Value;
                          






                    aDataTable.Rows.Add(dataRow);
                }
                ViewState["gv_Details_App"] = aDataTable;
                gv_ApprovalPathDetail.DataSource = aDataTable;
                gv_ApprovalPathDetail.DataBind();

                if (gv_ApprovalPathDetail.Rows.Count==0)
                {
                    lblstatus.Text = "No Approval Path have been  selected.";
                }
                else
                {
                    lblstatus.Text = "";
                }


                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {
                    DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].FindControl("ddlSequenceList");


                    ddlSequenceList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select One.....", String.Empty));
                    for (int k = 1; k < gv_ApprovalPathDetail.Rows.Count + 1; k++)
                    {
                        ddlSequenceList.Items.Insert(k, new ListItem(k.ToString()));
                    }
                }

                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {
                    DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("ddlSequenceList");
                    HiddenField hfSeq_No = (HiddenField)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("hfSeq_No");

                    ddlSequenceList.SelectedValue = hfSeq_No.Value;




                    HiddenField hfIsMinimumApprovalPerson = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfIsMinimumApprovalPerson"));
                    HiddenField hfCanEdit = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfCanEdit"));
                    HiddenField HiNotificationEmailApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("HiNotificationEmailApp"));
                    HiddenField hfNotificationSMSApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfNotificationSMSApp"));


                    CheckBox chkMimimumCount = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkMimimumCount");

                    if (hfIsMinimumApprovalPerson.Value!="")
                    {
                        chkMimimumCount.Checked =true;
                         
                    }


                    CheckBox chkIsEdit = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkIsEdit");

                    if (hfCanEdit.Value != "")
                    {
                        chkIsEdit.Checked = true;

                    }
                    CheckBoxList chkNotificationApp = (CheckBoxList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkNotificationApp");


                    if (HiNotificationEmailApp.Value != "")
                    {
                        chkNotificationApp.Items[0].Selected = true;

                    }

                    if (hfNotificationSMSApp.Value != "")
                    {
                        chkNotificationApp.Items[1].Selected = true;

                    }

                }
            }
            else
            {
                AlertMessageBoxShow("Already Exist !!!");
            }
        }
        
    }

    protected void rbLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbLocation.Items[0].Selected == true)
        {
            ddlOffice.Enabled = true;
            ddlLocation.Enabled = true;
            ddlFloor.Enabled = true;
            ddlMettingRoomName.Enabled = true;
            txtCapacity.ReadOnly = false;



            txtLocation.ReadOnly = true;
            txtDescription.ReadOnly = true;
            txtRemarks.ReadOnly = true;

            txtLocation.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtRemarks.Text = string.Empty;


        }

        if (rbLocation.Items[1].Selected == true)
        {
            ddlOffice.Enabled = false;
            ddlLocation.Enabled = false;
            ddlFloor.Enabled = false;
            ddlMettingRoomName.Enabled = false;
            txtCapacity.ReadOnly = true;

            txtLocation.ReadOnly = false;
            txtDescription.ReadOnly = false;
            txtRemarks.ReadOnly = true;



            ddlOffice.SelectedValue = null;
            ddlLocation.Items.Clear();
            ddlFloor.Items.Clear();
            ddlMettingRoomName.Items.Clear();
            txtCapacity.Text = string.Empty;
            txtRemarks.Text = string.Empty;

           



        }

        if (rbLocation.Items[2].Selected == true)
        {
            ddlOffice.Enabled = false;
            ddlLocation.Enabled = false;
            ddlFloor.Enabled = false;
            ddlMettingRoomName.Enabled = false;
            txtCapacity.ReadOnly = true;

            txtLocation.ReadOnly = true;
            txtDescription.ReadOnly = true;
            txtRemarks.ReadOnly = false;

            ddlOffice.SelectedValue = null;
            ddlLocation.Items.Clear();
            ddlFloor.Items.Clear();
            ddlMettingRoomName.Items.Clear();
            txtCapacity.Text = string.Empty;
            txtLocation.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        SaveUpdateInfo("");
    }

    public bool Validation()
    {

       
        if (ddlCompany.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(ddlCompany.SelectedValue) || ddlCompany.SelectedValue == "-1")
        {
            aShowMessage.ShowMessageBox("please fill out Company", this);
            ddlCompany.Focus();
            return false;
        }



        if (txtTitle.Text == "")
        {
            aShowMessage.ShowMessageBox("please fill out Title", this);
            txtTitle.Focus();
            return false;
        }

        if (ddlCategory.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(ddlCategory.SelectedValue) || ddlCategory.SelectedValue == "-1")
        {
            aShowMessage.ShowMessageBox("please fill out Meeting Category", this);
            ddlCategory.Focus();
            return false;
        }

        if (DivSubCommitte.Visible &&
            (ddlSubCommittee.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(ddlSubCommittee.SelectedValue) || ddlSubCommittee.SelectedValue == "-1"))
        {
            aShowMessage.ShowMessageBox("please fill out Sub-Committee Name", this);
            ddlSubCommittee.Focus();
            return false;
        }

        if (txtMeetingpurpose.Text == "")
        {
            aShowMessage.ShowMessageBox("please fill out Meeting Note", this);
            txtMeetingpurpose.Focus();
            return false;
        }

        if (txtMeetingDate.Text == "")
        {
            aShowMessage.ShowMessageBox("please fill out Meeting Date", this);
            txtMeetingDate.Focus();
            return false;
        }

        if (gv_DocumentUpload.Rows.Count == 0)
        {
            aShowMessage.ShowMessageBox("Please Add Document Information", this);
            return false;
        }


        for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
        {

            DropDownList ddlSequenceList =
                  (DropDownList)gv_ApprovalPathDetail.Rows[i].FindControl("ddlSequenceList");


            if (ddlSequenceList.SelectedValue == "")
            {
                aShowMessage.ShowMessageBox("please fill out Sequence", this);
                ddlSequenceList.Focus();
                return false;
            }
        }



        return true;
    }



    public bool CheckSeq()
    {

        for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
        {
            for (int j = 0; j < gv_ApprovalPathDetail.Rows.Count; j++)
            {
                if (i != j)
                {
                    DropDownList ddlSequenceList =
                 (DropDownList)gv_ApprovalPathDetail.Rows[i].FindControl("ddlSequenceList");
                    DropDownList ddlSequenceList2 =
                 (DropDownList)gv_ApprovalPathDetail.Rows[j].FindControl("ddlSequenceList");
                    if (ddlSequenceList.SelectedValue ==
                        ddlSequenceList2.SelectedValue)
                    {

                        return false;
                    }
                }
            }
        }

      
        return true;
    }
    private void SaveUpdateInfo(string acst)
    {
        if (Validation() == true)
        {
            if (CheckSeq())
            {
                string KeySearch = "";
                string MasterSearch = "";
                string DocumentSearch = "";
                string AgendaSearch = "";
                string MemberSearch = "";
                List<MiscellaneousInfoDocumentDAO> DocList = new List<MiscellaneousInfoDocumentDAO>();

                for (int i = 0; i < gv_DocumentUpload.Rows.Count; i++)
                {
                    HiddenField hfDocumentLink = (HiddenField) gv_DocumentUpload.Rows[i].FindControl("hfDocumentLink");
                    Label lbl_DocumentNote = (Label) gv_DocumentUpload.Rows[i].FindControl("lbl_DocumentNote");
                    HiddenField hfFileName = (HiddenField) gv_DocumentUpload.Rows[i].FindControl("hfFileName");
                    HiddenField hfExtractedText = (HiddenField) gv_DocumentUpload.Rows[i].FindControl("hfExtractedText");


                    MiscellaneousInfoDocumentDAO DocA = new MiscellaneousInfoDocumentDAO();
                    DocA.FileName = hfFileName.Value.ToString();
                    DocA.DocumentLink = hfDocumentLink.Value.ToString();
                    DocA.DocumentNote = lbl_DocumentNote.Text.Trim();
                    DocA.ExtractedText = hfExtractedText == null ? string.Empty : hfExtractedText.Value;
                    DocumentSearch = DocumentSearch + lbl_DocumentNote.Text.Trim() + " ";

                    DocList.Add(DocA);
                }


                List<MeetingInfoAgendaDAO> AgendaList = new List<MeetingInfoAgendaDAO>();

                for (int i = 0; i < gv_AgendaList.Rows.Count; i++)
                {
                    TextBox txtAgenda = (TextBox) gv_AgendaList.Rows[i].FindControl("txtAgenda");
                    TextBox txtObservation = (TextBox)gv_AgendaList.Rows[i].FindControl("txtObservation");
                    TextBox txtDecision = (TextBox)gv_AgendaList.Rows[i].FindControl("txtDecision");
                    TextBox txtRemarks = (TextBox)gv_AgendaList.Rows[i].FindControl("txtRemarks");
                    DropDownList ddlPresentor = (DropDownList)gv_AgendaList.Rows[i].FindControl("ddlPresentor");
                    DropDownList ddlImplementationStatus = (DropDownList)gv_AgendaList.Rows[i].FindControl("ddlImplementationStatus");


                    MeetingInfoAgendaDAO AgendaA = new MeetingInfoAgendaDAO();

                    AgendaA.Agenda = txtAgenda.Text.Trim().ToString();
                    AgendaA.Remarks = txtRemarks.Text.Trim().ToString();
                    AgendaA.Observation = txtObservation.Text.Trim().ToString();
                    AgendaA.Decision = txtDecision.Text.Trim().ToString();
                    AgendaSearch = AgendaSearch + txtAgenda.Text.Trim().ToString() + " ";

                    AgendaA.PresentorId =
                        Convert.ToInt32(ddlPresentor.SelectedIndex > 0 ? ddlPresentor.SelectedValue : null);

                    AgendaA.ImplementationStatus =
                        ddlImplementationStatus.SelectedValue;
                    AgendaList.Add(AgendaA);
                }

                int _RefEmpId = 0;
                int _RefSeqNo = 0;
                int _RefMinAppCount = 0;

                List<MiscellaneousInfoRoutingPathDAO> RoutingPath = new List<MiscellaneousInfoRoutingPathDAO>();

                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {
                    CheckBox chkMimimumCount = (CheckBox) gv_ApprovalPathDetail.Rows[i].FindControl("chkMimimumCount");
                    DropDownList ddlSequenceList =
                        (DropDownList) gv_ApprovalPathDetail.Rows[i].FindControl("ddlSequenceList");


                    HiddenField ApphfEmpInfoId =
                        (HiddenField) gv_ApprovalPathDetail.Rows[i].FindControl("ApphfEmpInfoId");


                    CheckBox chkIsEdit = (CheckBox) gv_ApprovalPathDetail.Rows[i].FindControl("chkIsEdit");
                    CheckBoxList chkNotificationApp =
                        (CheckBoxList) gv_ApprovalPathDetail.Rows[i].FindControl("chkNotificationApp");



                    MiscellaneousInfoRoutingPathDAO Routing = new MiscellaneousInfoRoutingPathDAO();


                    Routing.IsMinimumApprovalPerson = chkMimimumCount.Checked;
                    Routing.Seq_No = Convert.ToInt32(ddlSequenceList.SelectedValue);
                    Routing.EmpInfoId = Convert.ToInt32(ApphfEmpInfoId.Value.Trim());

                    Routing.CanEdit = chkIsEdit.Checked;
                    Routing.IsEmailNotification = chkNotificationApp.Items[0].Selected;
                    Routing.IsSMSNotification = chkNotificationApp.Items[1].Selected;
                    if (chkMimimumCount.Checked)
                    {
                        _RefMinAppCount++;
                        if (Routing.Seq_No == 1)
                        {
                            _RefEmpId = Convert.ToInt32(ApphfEmpInfoId.Value.Trim());
                            _RefSeqNo = (int) Routing.Seq_No;
                        }
                    }

                    RoutingPath.Add(Routing);
                }





                List<MeetingInfoDetailDAO> MeetingInfoDetailList = new List<MeetingInfoDetailDAO>();

                for (int i = 0; i < gv_Details_Save.Rows.Count; i++)
                {
                    HiddenField hfBMemberSetupDetailsID =
                        ((HiddenField) gv_Details_Save.Rows[i].FindControl("hfBMemberSetupDetailsID"));
                    HiddenField hfIsBoardMember = ((HiddenField) gv_Details_Save.Rows[i].FindControl("hfIsBoardMember"));
                    HiddenField hfType = ((HiddenField) gv_Details_Save.Rows[i].FindControl("hfType"));
                    TextBox txt_EmpMasterCode = ((TextBox) gv_Details_Save.Rows[i].FindControl("txt_EmpMasterCode"));
                    HiddenField ShfEmpInfoId = ((HiddenField) gv_Details_Save.Rows[i].FindControl("ShfEmpInfoId"));
                    TextBox txt_EmpName = (TextBox) gv_Details_Save.Rows[i].FindControl("txt_EmpName");
                    TextBox txt_Designation = (TextBox) gv_Details_Save.Rows[i].FindControl("txt_Designation");

                    HiddenField hfNotification = ((HiddenField) gv_Details_Save.Rows[i].FindControl("hfNotification"));
                    HiddenField hfPosition = ((HiddenField) gv_Details_Save.Rows[i].FindControl("hfPosition"));


                    //DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
                    RadioButtonList rbType = ((RadioButtonList) gv_Details_Save.Rows[i].FindControl("rbType"));
                    CheckBoxList chkNotification =
                        ((CheckBoxList) gv_Details_Save.Rows[i].FindControl("chkNotification"));
                    RadioButtonList chkPosition = ((RadioButtonList) gv_Details_Save.Rows[i].FindControl("chkPosition"));




                    MeetingInfoDetailDAO MeetingInfoDetail = new MeetingInfoDetailDAO();


                    MeetingInfoDetail.Type = rbType.SelectedValue;

                    if (rbType.SelectedValue == "Employee")
                    {

                        try
                        {
                            MeetingInfoDetail.EmpInfoId = Convert.ToInt32(ShfEmpInfoId.Value);
                        }
                        catch (Exception)
                        {

                            MeetingInfoDetail.EmpInfoId = null;
                        }
                    }
                    else
                    {
                        MeetingInfoDetail.EmpInfoId = null;




                    }

                    MeetingInfoDetail.EmpMasterCode = (txt_EmpMasterCode.Text.Trim());
                    MeetingInfoDetail.IsBoardMember = "0";

                    MeetingInfoDetail.BMemberSetupDetailsID = (hfBMemberSetupDetailsID.Value.Trim());
                    MeetingInfoDetail.EmpName = (txt_EmpName.Text.Trim());
                    MeetingInfoDetail.Designation = (txt_Designation.Text.Trim());
                    MeetingInfoDetail.NotificationEmail = (chkNotification.Items[0].Selected);
                    MeetingInfoDetail.NotificationSMS = (chkNotification.Items[1].Selected);
                    try
                    {
                         MeetingInfoDetail.Position = chkPosition.SelectedItem.Text;
                    }
                    catch (Exception)
                    {

                        MeetingInfoDetail.Position = "";
                    }
                   

                    MemberSearch = MemberSearch + txt_EmpName.Text.Trim() + " ";


                    if (chkNotification.Items[0].Selected==true)
                    {
                        try
                        {
                            if (acst != "Drafted")
                            {
                                // Send email asynchronously so it doesn't block the save operation
                                int empId = Convert.ToInt32(MeetingInfoDetail.EmpInfoId);
                                System.Threading.Tasks.Task.Run(() =>
                                    SenMailForApprved(empId,
                                        " Meeting Information ", @"  <br/> Dear Sir, <br/>
Meeting Entry Demo Mail.<br/><br/>
 please login for the details from the below link.<br/><br/>   http://182.160.103.234:8088/
"));
                            }
                        }
                        catch (Exception)
                        {
                            
                            //throw;
                        }
                    }


                    MeetingInfoDetailList.Add(MeetingInfoDetail);
                }

                List<MeetingInfoDetailDAO> MeetingInfoDetailList2 = new List<MeetingInfoDetailDAO>();


                for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
                {

                    TextBox txtBoardMember_EmpName = (TextBox)gv_BoardMember.Rows[i].FindControl("txtBoardMember_EmpName");
                    TextBox txtBoardMember_Designation = (TextBox)gv_BoardMember.Rows[i].FindControl("txtBoardMember_Designation");
                    HiddenField hfBMemberSetupDetailsIDb = ((HiddenField)gv_BoardMember.Rows[i].FindControl("hfBMemberSetupDetailsIDb"));


                    DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");

                    RadioButtonList chkBoardMemberPosition = ((RadioButtonList)gv_BoardMember.Rows[i].FindControl("chkBoardMemberPosition"));

                    MeetingInfoDetailDAO MeetingInfoDetail = new MeetingInfoDetailDAO();
                     
                    MemberSearch = MemberSearch + txtBoardMember_EmpName.Text.Trim() + " ";

                    MeetingInfoDetail.Type = "";

                    MeetingInfoDetail.EmpInfoId =null;
                        

                    MeetingInfoDetail.EmpMasterCode ="";
                    MeetingInfoDetail.IsBoardMember = "1";

                    MeetingInfoDetail.BMemberSetupDetailsID = (hfBMemberSetupDetailsIDb.Value.Trim());
                    MeetingInfoDetail.EmpName = (txtBoardMember_EmpName.Text.Trim());
                    MeetingInfoDetail.Designation = (txtBoardMember_Designation.Text.Trim());
                    MeetingInfoDetail.NotificationEmail =false;
                    MeetingInfoDetail.NotificationSMS = false;
                    //MeetingInfoDetail.Position = (chkBoardMemberPosition.SelectedValue);
                    MeetingInfoDetail.Position = (ddlPosition.SelectedItem.Text);

                    MeetingInfoDetailList2.Add(MeetingInfoDetail);
                }


                MeetingEntryDAO aMaster = new MeetingEntryDAO();

                aMaster.MeetingInfoID = id_mastetID.Value == "" ? 0 : Convert.ToInt32(id_mastetID.Value);

                aMaster.CompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
                string classfic = "";
                //if (ddlClassification.SelectedValue != "")
                //{
                //    classfic = ddlCategory.SelectedItem.Text.Trim();
                //}

                MasterSearch = MasterSearch + txtTitle.Text.Trim() + " "
                               +
                               txtMeetingpurpose.Text.Trim() + " " +
                               classfic + " "
                               + txtMeetingDate.Text.Trim() + " " + txtStartTime.Text.Trim() + "" +
                               " " + txtEndTime.Text.Trim();
                aMaster.Title = txtTitle.Text.Trim();

                aMaster.CategoryID = Convert.ToInt32(ddlCategory.SelectedIndex > 0 ? ddlCategory.SelectedValue : null);

                aMaster.MeetingPurpose = txtMeetingpurpose.Text.Trim();
                aMaster.Classification = null; // ddlClassification.SelectedIndex > 0 ? ddlClassification.SelectedValue : null;
                aMaster.MeetingDate = string.IsNullOrEmpty(txtMeetingDate.Text)
                    ? (DateTime?) null
                    : DateTime.Parse(txtMeetingDate.Text).Date;

                aMaster.StartTime = string.IsNullOrEmpty(txtStartTime.Text)
                    ? (TimeSpan?) null
                    : TimeSpan.Parse(txtStartTime.Text);

                aMaster.EndTime = string.IsNullOrEmpty(txtEndTime.Text)
                    ? (TimeSpan?) null
                    : TimeSpan.Parse(txtEndTime.Text);



                aMaster.SubCommitteeId =
                    Convert.ToInt32(ddlSubCommittee.SelectedIndex > 0 ? ddlSubCommittee.SelectedValue : null);


                aMaster.IsOfficePremisis = rbLocation.Items[0].Selected;
                aMaster.IsOuterPremisis = rbLocation.Items[1].Selected;
                aMaster.IsVirtualMeeting = rbLocation.Items[2].Selected;


                if (rbLocation.Items[0].Selected == true)
                {

                    aMaster.OfficeId = Convert.ToInt32(ddlOffice.SelectedIndex > 0 ? ddlOffice.SelectedValue : null);
                    aMaster.LocationId =
                        Convert.ToInt32(ddlLocation.SelectedIndex > 0 ? ddlLocation.SelectedValue : null);
                    aMaster.FloorId = Convert.ToInt32(ddlFloor.SelectedIndex > 0 ? ddlFloor.SelectedValue : null);
                    aMaster.MettingRoomId =
                        Convert.ToInt32(ddlMettingRoomName.SelectedIndex > 0 ? ddlMettingRoomName.SelectedValue : null);
                    aMaster.Location = null;
                    aMaster.LocationDescription = null;

                }

                if (rbLocation.Items[1].Selected == true)
                {

                    aMaster.OfficeId = null;
                    aMaster.LocationId = null;
                    aMaster.FloorId = null;
                    aMaster.MettingRoomId = null;
                    aMaster.Location = txtLocation.Text;
                    aMaster.LocationDescription = txtDescription.Text;


                }

                if (rbLocation.Items[2].Selected == true)
                {

                    aMaster.OfficeId = null;
                    aMaster.LocationId = null;
                    aMaster.FloorId = null;
                    aMaster.MettingRoomId = null;
                    aMaster.Location = null;
                    aMaster.LocationDescription = null;
                    aMaster.Remarks = txtRemarks.Text;


                }

                KeySearch = MasterSearch + " " + MemberSearch + " " + DocumentSearch + " " + AgendaSearch;
                aMaster.KeySearch = KeySearch;




                aMaster.RefMinAppCountCheck = 0;


                 
                    if(rbNotice.Items[0].Selected)
                {
                    aMaster.IsNotice = true;
                }
                    else
                    {
                        aMaster.IsNotice = false;
                        
                    }
              
                aMaster.Isapproved = false;

                

                if (acst == "Drafted")
                {
                    aMaster.ActionStatus = acst;
                    aMaster.RefEmpId = 0;
                    aMaster.RefSeqNo = 0;
                    aMaster.RefMinAppCount = 0;
                }
                else
                {
                    aMaster.RefEmpId = _RefEmpId;
                    aMaster.RefSeqNo = _RefSeqNo;
                    aMaster.RefMinAppCount = _RefMinAppCount;


                    if (gv_ApprovalPathDetail.Rows.Count==0)
                    {
                        aMaster.Isapproved = true;
                        aMaster.RefEmpId = 0;
                        aMaster.ActionStatus = "Approved";
                    }
                    else
                    {
                        aMaster.ActionStatus = "Initiator";
                    }


                  
                }
                bool result = false;
                int pk = AMeetingEntryDal.SaveMaster(aMaster, Session["UserId"].ToString());


                if (pk > 0)
                {

                    result = true;
                    AMeetingEntryDal.SaveDetails(MeetingInfoDetailList, pk, 0);
                    AMeetingEntryDal.SaveDetails(MeetingInfoDetailList2, pk,1);
                    AMeetingEntryDal.SaveRoutingPathDetails(RoutingPath, pk);
                    AMeetingEntryDal.SaveDocumentDetails(DocList, pk);
                    AMeetingEntryDal.SaveAgendaDetails(AgendaList, pk);


                    if (gv_ApprovalPathDetail.Rows.Count > 0)
                    {


                        try
                        {
                            if (Session["EmpInfoId"].ToString() != "")
                            {

                                if (acst != "Drafted")
                                {


                                    //del Log
                                    if (aMaster.MeetingInfoID > 0)
                                    {
                                        bool status = AMeetingEntryDal.AuditTrailLogById(pk.ToString(), "Edit");
                                    }
                                    else
                                    {
                                        bool status = AMeetingEntryDal.AuditTrailLogById(pk.ToString(), "Initial");

                                    }


                                    if (gv_ApprovalPathDetail.Rows.Count > 0)
                                    {

                                        MeetingInfoAppLogIdDAO appLogDaoa = new MeetingInfoAppLogIdDAO();

                                        appLogDaoa.ActionStatus = "Drafted";
                                        appLogDaoa.ApprovedDate = DateTime.Now;
                                        appLogDaoa.ApprovedBy = Convert.ToInt32(Session["UserId"].ToString());
                                        appLogDaoa.PreEmpInfoId = Convert.ToInt32(0);
                                        appLogDaoa.ForEmpInfoId = Convert.ToInt32(Session["EmpInfoId"].ToString());
                                        appLogDaoa.MeetingInfoID = pk;

                                        appLogDaoa.Comments = "";


                                        int idd = AMeetingEntryDal.SavAppLog(appLogDaoa);


                                        //DataTable dtempdata =
                                        //    AMAsterDal.GetEmpRoutingPath(pk.ToString());
                                        MeetingInfoAppLogIdDAO appLogDao = new MeetingInfoAppLogIdDAO();
                                        {



                                            appLogDao.ActionStatus = "Initiator";
                                            appLogDao.ApprovedDate = DateTime.Now;
                                            appLogDao.ApprovedBy = Convert.ToInt32(Session["UserId"].ToString());
                                            appLogDao.PreEmpInfoId = Convert.ToInt32(Session["EmpInfoId"].ToString());
                                            appLogDao.ForEmpInfoId = Convert.ToInt32(_RefEmpId);
                                            appLogDao.MeetingInfoID = pk;

                                            appLogDao.Comments = "";


                                        }
                                        ;
                                        int iddddd = AMeetingEntryDal.SavAppLog(appLogDao);
                                        //                            SenMailForApprved(appLogDao.ForEmpInfoId, " Increment Approval ", @"  <br/> Dear Sir, <br/>
                                        //An Increment is waiting for your approval. <br/><br/>
                                        // please login for the details from the below link.<br/><br/>   http://182.160.103.234:8088/
                                        //");

                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                            //throw;
                        }

                    }


                }

                else
                {
                    result = false;
                }

                if (result == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "alert",
                        "alert('Operation Successful...');window.location ='MeetingEntry.aspx';",
                        true);
                }
                else
                {
                    AlertMessageBoxShow("Operation Failed");
                }
            }
            else
            {
                AlertMessageBoxShow("Sequence No Can not be Same");
            }
        }
    }





    public static bool SenMailForApprved(int forEmpID, string mSubject, string mBody)
    {



        string ForMailAddress = "";
        using (var db = new HRIS_SMCEntities())
        {
            var GetMailAddress = (from t in db.tblEmpGeneralInfoes
                                  where t.EmpInfoId == forEmpID
                                  select t).FirstOrDefault();

            if (GetMailAddress != null)
            {
                ForMailAddress = GetMailAddress.OfficialEmail;
            }



        }

        if (ForMailAddress != "")
        {
            try
            {
                // Set TLS 1.2 (Office 365 requires this)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (SmtpClient smtpClient = new SmtpClient("shuvosmtp.office365.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;

                    // Use your actual Office 365 credentials
                    smtpClient.Credentials = new NetworkCredential("shuvono-reply@smc-bd.org", "vfwzmbxprdmqhhln");

                    // Set timeout (in milliseconds)
                    smtpClient.Timeout = 20000;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("shuvono-reply@smc-bd.org");
                        mailMessage.IsBodyHtml = true;
                        mailMessage.To.Add(ForMailAddress);
                        mailMessage.Subject = mSubject;
                        mailMessage.Body =
                   "<div style='background-color: #DFF0D8; border-style: solid; border-color: #39B3D7; color: black; padding: 25px; border-radius: 15px 50px 30px 5px;'> <br/>" +
                   WebUtility.HtmlDecode(mBody)
                   +
                   "</div>";
                        mailMessage.IsBodyHtml = true;

                        smtpClient.Send(mailMessage);

                    }
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {

                }
            }





            System.Threading.Thread.Sleep(100);
        }


        return true;
    }

    protected void editButton_OnClick(object sender, EventArgs e)
    {
        SaveUpdateInfo("");
    }

    protected void delButton_OnClick(object sender, EventArgs e)
    {
        if (id_mastetID.Value != "")
        {
            // int Id = Convert.ToInt32(Session["UserId"].ToString());
            //Int32 DEl_ID = SaveChangesMasterDelete(Id);


            bool status = AMeetingEntryDal.AuditTrailLogById(id_mastetID.Value, "Delete");
            bool ss = AMeetingEntryDal.DeleteMaster(id_mastetID.Value);
            if (ss)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
            "alert",
            "alert('Operation Successful...');window.location ='MeetingStatusList.aspx';",
            true);
            }
            else
            {
                AlertMessageBoxShow("Operation Faild!!!!");
            }


        }
    }

    protected void detailsViewButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("MeetingStatusList.aspx");
    }

    protected void ddlOffice_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlOffice.SelectedIndex > 0)
        {
            using (DataTable dt = _commonDataLoad.GetDDLJobLocation(ddlOffice.SelectedValue))
            {
                ddlLocation.DataSource = dt;
                ddlLocation.DataValueField = "Value";
                ddlLocation.DataTextField = "TextField";
                ddlLocation.DataBind();

            }
        }
        else
        {
            ddlLocation.Items.Clear();
            ddlFloor.Items.Clear();
            ddlMettingRoomName.Items.Clear();
            txtCapacity.Text = "";
        }
    }

    protected void ddlLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLocation.SelectedIndex > 0)
        {
            using (DataTable dt = _commonDataLoad.GetDDLFloor(ddlLocation.SelectedValue))
            {
                ddlFloor.DataSource = dt;
                ddlFloor.DataValueField = "Value";
                ddlFloor.DataTextField = "TextField";
                ddlFloor.DataBind();
            }
        }

        else
        {
            ddlFloor.Items.Clear();
            ddlMettingRoomName.Items.Clear();
            txtCapacity.Text = "";
        }
    }

    protected void ddlFloor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFloor.SelectedIndex > 0)
        {
            using (DataTable dt = _commonDataLoad.GetDDLMeetingRoom(ddlFloor.SelectedValue))
            {
                ddlMettingRoomName.DataSource = dt;
                ddlMettingRoomName.DataValueField = "Value";
                ddlMettingRoomName.DataTextField = "TextField";
                ddlMettingRoomName.DataBind();
            }
        }

        else
        {
          
            ddlMettingRoomName.Items.Clear();
            txtCapacity.Text = "";
        }
    }

    protected void ddlMettingRoomName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMettingRoomName.SelectedIndex > 0)
        {
            DataTable dtMaster = AMeetingEntryDal.GetCapacityDataByMeetingId(ddlMettingRoomName.SelectedValue);
            if (dtMaster.Rows.Count > 0)
            {
                txtCapacity.Text = dtMaster.Rows[0]["MeetingRoomCapacity"].ToString();
            }
        }
        else
        {

            
            txtCapacity.Text = "";
        }
    }

    protected void btn_DetailsAdd_OnClick(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;

        DataTable aTable = new DataTable();

        aTable.Columns.Add("Type");
        aTable.Columns.Add("CompanyId");

        aTable.Columns.Add("EmpMasterCode");
        aTable.Columns.Add("EmpInfoId");
        aTable.Columns.Add("EmpName");
        aTable.Columns.Add("Designation");
        aTable.Columns.Add("NotificationEmail");
        aTable.Columns.Add("NotificationSMS");
        aTable.Columns.Add("Position");
        aTable.Columns.Add("IsBoardMember");
        aTable.Columns.Add("BMemberSetupDetailsID");
        
        

        DataRow dr;



        for (int i = 0; i < gv_Details_Save.Rows.Count; i++)
        {
            dr = aTable.NewRow();
            HiddenField hfBMemberSetupDetailsID = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfBMemberSetupDetailsID"));
            HiddenField hfIsBoardMember = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfIsBoardMember"));
            HiddenField hfType = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfType"));
            TextBox txt_EmpMasterCode = ((TextBox)gv_Details_Save.Rows[i].FindControl("txt_EmpMasterCode"));
            HiddenField ShfEmpInfoId = ((HiddenField)gv_Details_Save.Rows[i].FindControl("ShfEmpInfoId"));
            TextBox txt_EmpName = (TextBox)gv_Details_Save.Rows[i].FindControl("txt_EmpName");
            TextBox txt_Designation = (TextBox)gv_Details_Save.Rows[i].FindControl("txt_Designation");

            HiddenField HiNotificationEmail = ((HiddenField)gv_Details_Save.Rows[i].FindControl("HiNotificationEmail"));
            HiddenField hfNotificationSMS = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfNotificationSMS"));
            HiddenField hfPosition = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfPosition"));



            RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType"));
            CheckBoxList chkNotification = ((CheckBoxList)gv_Details_Save.Rows[i].FindControl("chkNotification"));
            RadioButtonList chkPosition = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("chkPosition"));






            dr["IsBoardMember"] = hfIsBoardMember.Value;
            dr["BMemberSetupDetailsID"] = hfBMemberSetupDetailsID.Value;

            DropDownList ddlEmployeeSave = (DropDownList)gv_Details_Save.Rows[i].FindControl("ddlEmployeeSave");
            dr["EmpInfoId"] = ddlEmployeeSave != null ? ddlEmployeeSave.SelectedValue : "";

            dr["EmpMasterCode"] = txt_EmpMasterCode.Text;
            dr["EmpName"] = txt_EmpName.Text;
            dr["Designation"] = txt_Designation.Text;
            dr["Type"] = rbType != null ? rbType.SelectedValue : "";
            
            RadioButtonList ddlCompanySave = (RadioButtonList)gv_Details_Save.Rows[i].FindControl("ddlCompanySave");
            dr["CompanyId"] = ddlCompanySave != null ? ddlCompanySave.SelectedValue : "";
            
            if (chkNotification != null && chkNotification.Items.Count > 1)
            {
                dr["NotificationEmail"] = chkNotification.Items[0].Selected.ToString();
                dr["NotificationSMS"] = chkNotification.Items[1].Selected.ToString();
            }
            else
            {
                dr["NotificationEmail"] = "False";
                dr["NotificationSMS"] = "False";
            }

            dr["Position"] = chkPosition != null ? chkPosition.SelectedValue : "";
                   






            aTable.Rows.Add(dr);

            if (rowIndex == i)
            {
                dr = aTable.NewRow();
              

                dr["EmpInfoId"] = "";

                dr["EmpMasterCode"] = "";
                dr["EmpName"] = "";
                dr["Designation"] = "";
                 hfIsBoardMember.Value="2";
                dr["IsBoardMember"] = hfIsBoardMember.Value;

                hfBMemberSetupDetailsID.Value="0";
                dr["BMemberSetupDetailsID"] = hfBMemberSetupDetailsID.Value;


                hfType.Value = "Guest";

                dr["Type"] = hfType.Value.Trim();
                if (dr["Type"].ToString() != "")
                {

                    rbType.SelectedValue = hfType.Value.Trim();
                }
                dr["NotificationEmail"] = "";
                dr["NotificationSMS"] = "";
                chkNotification.SelectedValue = null;

                dr["Position"] = "";
                chkPosition.SelectedValue = null;

                aTable.Rows.Add(dr);
            }
        }

        //Session["table"] = (DataTable)aTable;
        gv_Details_Save.DataSource = null;
        gv_Details_Save.DataBind();
        gv_Details_Save.DataSource = aTable;
        ViewState["gv_Details_List"] = aTable;

        gv_Details_Save.DataBind();

        //using (DataTable dt2 = AMeetingEntryDal.GetEmpInfobyID(ddlCompany.SelectedValue))
        //{

        for (int i = 0; i < gv_Details_Save.Rows.Count; i++)
        {
               RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType"));
             rbType.SelectedValue = ((HiddenField)gv_Details_Save.Rows[i].Cells[1].FindControl("hfType"))
                     .Value;



             CheckBoxList chkNotification = ((CheckBoxList)gv_Details_Save.Rows[i].FindControl("chkNotification"));

             HiddenField HiNotificationEmail = ((HiddenField)gv_Details_Save.Rows[i].FindControl("HiNotificationEmail"));
             HiddenField hfNotificationSMS = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfNotificationSMS"));

             if (HiNotificationEmail.Value!="")
            {
                try
                {
                    chkNotification.Items[0].Selected = Convert.ToBoolean(HiNotificationEmail.Value);
                }
                catch (Exception)
                {
                    
                    //throw;
                }
                 
            }

             if (hfNotificationSMS.Value != "")
             {
                 try
                 {
                     chkNotification.Items[1].Selected = Convert.ToBoolean(hfNotificationSMS.Value);
                 }
                 catch (Exception)
                 {
                     
                     //throw;
                 }

             }


             RadioButtonList chkPosition = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("chkPosition"));

             HiddenField hfPosition = ((HiddenField)gv_Details_Save.Rows[i].FindControl("hfPosition"));

             chkPosition.SelectedValue = hfPosition.Value;
             
        }
        //}chkNotification_OnSelectedIndexChanged

    }

    protected void rbType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((RadioButtonList)sender).Parent.Parent)).RowIndex;

        RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[rowIndex].FindControl("rbType"));
        HiddenField hfType = ((HiddenField)gv_Details_Save.Rows[rowIndex].FindControl("hfType"));
        RadioButtonList ddlCompanySave = ((RadioButtonList)gv_Details_Save.Rows[rowIndex].FindControl("ddlCompanySave"));
        DropDownList ddlEmployeeSave = ((DropDownList)gv_Details_Save.Rows[rowIndex].FindControl("ddlEmployeeSave"));
        TextBox txt_EmpName = ((TextBox)gv_Details_Save.Rows[rowIndex].FindControl("txt_EmpName"));

        hfType.Value = rbType.SelectedValue;

        if (rbType.SelectedValue == "Guest")
        {
            if (ddlCompanySave != null)
            {
                ddlCompanySave.ClearSelection();
                ddlCompanySave.Enabled = false;
            }
            if (ddlEmployeeSave != null) ddlEmployeeSave.Visible = false;
            if (txt_EmpName != null) txt_EmpName.Visible = true;
        }
        else
        {
            if (ddlCompanySave != null) ddlCompanySave.Enabled = true;
            if (ddlEmployeeSave != null) ddlEmployeeSave.Visible = true;
            if (txt_EmpName != null) txt_EmpName.Visible = false;
        }
    }

    protected void chkNotification_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((CheckBoxList)sender).Parent.Parent)).RowIndex;

        CheckBoxList chkNotification = ((CheckBoxList)gv_Details_Save.Rows[rowIndex].FindControl("chkNotification"));

        HiddenField HiNotificationEmail = ((HiddenField)gv_Details_Save.Rows[rowIndex].FindControl("HiNotificationEmail"));
        HiddenField hfNotificationSMS = ((HiddenField)gv_Details_Save.Rows[rowIndex].FindControl("hfNotificationSMS"));

        HiNotificationEmail.Value = chkNotification.Items[0].Selected.ToString();
        hfNotificationSMS.Value = chkNotification.Items[1].Selected.ToString();

    }

    protected void chkPosition_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((RadioButtonList)sender).Parent.Parent)).RowIndex;

        RadioButtonList chkPosition = ((RadioButtonList)gv_Details_Save.Rows[rowIndex].FindControl("chkPosition"));

        HiddenField hfPosition = ((HiddenField)gv_Details_Save.Rows[rowIndex].FindControl("hfPosition"));

        hfPosition.Value = chkPosition.SelectedValue;
    }

    protected void chkNotificationApp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow) (((CheckBoxList) sender).Parent.Parent)).RowIndex;

        CheckBoxList chkNotificationApp =
            ((CheckBoxList) gv_ApprovalPathDetail.Rows[rowIndex].FindControl("chkNotificationApp"));

        HiddenField HiNotificationEmailApp =
            ((HiddenField) gv_ApprovalPathDetail.Rows[rowIndex].FindControl("HiNotificationEmailApp"));
        HiddenField hfNotificationSMSApp =
            ((HiddenField) gv_ApprovalPathDetail.Rows[rowIndex].FindControl("hfNotificationSMSApp"));

        HiNotificationEmailApp.Value = chkNotificationApp.Items[0].Selected.ToString();
        hfNotificationSMSApp.Value = chkNotificationApp.Items[1].Selected.ToString();


    }


    protected void chkIsEdit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;

        CheckBox chkIsEdit = ((CheckBox)gv_ApprovalPathDetail.Rows[rowIndex].FindControl("chkIsEdit"));

        HiddenField hfCanEdit = ((HiddenField)gv_ApprovalPathDetail.Rows[rowIndex].FindControl("hfCanEdit"));


        hfCanEdit.Value = chkIsEdit.Checked.ToString();
        
    }

    protected void chkMimimumCount_OnCheckedChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;

        CheckBox chkMimimumCount = ((CheckBox)gv_ApprovalPathDetail.Rows[rowIndex].FindControl("chkMimimumCount"));

        HiddenField hfIsMinimumApprovalPerson = ((HiddenField)gv_ApprovalPathDetail.Rows[rowIndex].FindControl("hfIsMinimumApprovalPerson"));


        hfIsMinimumApprovalPerson.Value = chkMimimumCount.Checked.ToString();
    }
    protected void ddlComSearch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDivision.Items.Clear();
        ddlDepartment.Items.Clear();
        ddlEmp.Items.Clear();
        if (ddlComSearch.SelectedValue != "")
        {
            using (DataTable dt = AMAsterDal.GetDDLComDivision(ddlComSearch.SelectedValue))
            {
                ddlDivision.DataSource = dt;
                ddlDivision.DataValueField = "Value";
                ddlDivision.DataTextField = "TextField";
                ddlDivision.DataBind();
            }


            using (DataTable dt = AMAsterDal.GetDDLEmpInfo(ddlComSearch.SelectedValue))
            {
                ddlEmp.DataSource = dt;
                ddlEmp.DataValueField = "Value";
                ddlEmp.DataTextField = "TextField";
                ddlEmp.DataBind();
            }
        }
        else
        {
            ddlDivision.Items.Clear();
            ddlDepartment.Items.Clear();
        }

    }
    protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DivSubCommitte.Visible = false;
        ddlSubCommittee.Items.Clear();

        string effectiveCategory = ddlCategory.SelectedValue == "" ? "1" : ddlCategory.SelectedValue;

        if (effectiveCategory == "1")
        {
            DataTable jobCreationInfos = new DataTable();
            jobCreationInfos = AMeetingEntryDal.GetEmpMemberInfoByCategory(ddlCompany.SelectedValue);
            if (jobCreationInfos.Rows.Count > 0)
            {
                ViewState["gv_BoardMember_List"] = jobCreationInfos;
                gv_BoardMember.DataSource = jobCreationInfos;
                gv_BoardMember.DataBind();
                DataTable dtMemberPostion = MemberPositionCache;

                for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
                {
                    DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
                    ddlPosition.DataSource = dtMemberPostion;
                    ddlPosition.DataValueField = "Value";
                    ddlPosition.DataTextField = "TextField";
                    ddlPosition.DataBind();

                    // Fix: use row index i, not inner loop over all rows (was O(n²))
                    try
                    {
                        if (i < jobCreationInfos.Rows.Count)
                            ddlPosition.SelectedValue = jobCreationInfos.Rows[i]["PositionId"].ToString();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                if (Session["MeetingMastetID"] != null && ddlCategory.SelectedValue == "")
                {
                    DataTable dt = (DataTable)ViewState["gv_BoardMember_List"];
                    gv_BoardMember.DataSource = dt;
                    gv_BoardMember.DataBind();
                }
                else
                {
                    ViewState["gv_BoardMember_List"] = null;
                    gv_BoardMember.DataSource = null;
                    gv_BoardMember.DataBind();
                    LoadInitialGridDetails_Save();
                }
            }
        }
        else
        {
            ViewState["gv_BoardMember_List"] = null;
            gv_BoardMember.DataSource = null;
            gv_BoardMember.DataBind();
            LoadInitialGridBoardMember();
        }

        if (ddlCategory.SelectedValue != "" && ddlCategory.SelectedValue != "1")
        {
            DivSubCommitte.Visible = true;

            if (id_mastetID.Value=="")
            {
                using (DataTable dt = AMeetingEntryDal.GetDDLComDivisionEntry(ddlCompany.SelectedValue, ddlCategory.SelectedValue))
                {
                    if (dt.Rows.Count>0)
                    {
                        ddlSubCommittee.DataSource = dt;
                        ddlSubCommittee.DataValueField = "Value";
                        ddlSubCommittee.DataTextField = "TextField";
                        ddlSubCommittee.DataBind();
                    }
                    else
                    {
                        DivSubCommitte.Visible = false;
                    }
                }
            }
            else
            {
                using (DataTable dt = AMeetingEntryDal.GetDDLComDivision(ddlCompany.SelectedValue, ddlCategory.SelectedValue))
                {
                    ddlSubCommittee.DataSource = dt;
                    ddlSubCommittee.DataValueField = "Value";
                    ddlSubCommittee.DataTextField = "TextField";
                    ddlSubCommittee.DataBind();
                }
            }
            LoadInitialGridDetails_Save();
        }
    }

    protected void ddlSubCommittee_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedValue != "")
        {

        DataTable dtMemberPostion = MemberPositionCache;

            DataTable jobCreationInfos = new DataTable();
                jobCreationInfos = AMeetingEntryDal.GetEmpMemberInfoBySubCOmmitte(ddlCompany.SelectedValue,
                    ddlSubCommittee.SelectedValue);
                if (jobCreationInfos.Rows.Count > 0)
                {

                    ViewState["gv_BoardMember_List"] = jobCreationInfos;
                    gv_BoardMember.DataSource = jobCreationInfos;
                    gv_BoardMember.DataBind();
                for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
                {
                    DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
                    ddlPosition.DataSource = dtMemberPostion;
                    ddlPosition.DataValueField = "Value";
                    ddlPosition.DataTextField = "TextField";
                    ddlPosition.DataBind();

                    // Fix: use row index i, not inner loop over all rows (was O(n²))
                    try
                    {
                        if (i < jobCreationInfos.Rows.Count)
                            ddlPosition.SelectedValue = jobCreationInfos.Rows[i]["PositionId"].ToString();
                    }
                    catch (Exception)
                    {
                    }
                }

                }
                else
                {
                    ViewState["gv_BoardMember_List"] = null;
                    gv_BoardMember.DataSource = null;
                    gv_BoardMember.DataBind();
                    LoadInitialGridBoardMember();
                }

            DataTable dtMember_List = AMAster.GetMemberListDataById(ddlSubCommittee.SelectedValue);

            if (dtMember_List.Rows.Count > 0)
            {

                ViewState["gv_Details_List"] = dtMember_List;
                gv_Details_Save.DataSource = dtMember_List;
                gv_Details_Save.DataBind();
                for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
                {
                    try
                    {
                        RadioButtonList rbType = ((RadioButtonList)gv_Details_Save.Rows[i].FindControl("rbType"));
                        rbType.SelectedValue = ((HiddenField)gv_Details_Save.Rows[i].Cells[1].FindControl("hfType"))
                                .Value;
                    }
                    catch(Exception ex)
                    {

                    }
                }

                }
            else
            {
                ViewState["gv_Details_List"] = null;
                gv_Details_Save.DataSource = null;
                gv_Details_Save.DataBind();
            LoadInitialGridDetails_Save();

            }

        }
    }

    protected void lbDraft_OnClick(object sender, EventArgs e)
    {
        string acStatus = "Drafted";
        SaveUpdateInfo(acStatus);
    }

    protected void AddModalBoardMember_OnClick(object sender, EventArgs e)
    {
        if (BordmemberMemberValidationApp())
        {
            if (CheckBoardMemberApp())
            {

                DataTable aDataTable = new DataTable();
                aDataTable.Columns.Add("EmpInfoId");
                aDataTable.Columns.Add("EmpMasterCode");
                aDataTable.Columns.Add("EmpName");
                aDataTable.Columns.Add("Designation");
                aDataTable.Columns.Add("Seq_No");
                aDataTable.Columns.Add("IsMinimumApprovalPerson");
                aDataTable.Columns.Add("CanEdit");
                aDataTable.Columns.Add("NotificationEmail");
                aDataTable.Columns.Add("NotificationSMS");




                DataRow dataRow = null;

                for (int i = 0; i < gv_loadGridView.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)gv_loadGridView.Rows[i].Cells[0].FindControl("BchkSelect");


                    Label BM_Name = (Label)gv_loadGridView.Rows[i].FindControl("BM_Name");
               

                    if (chkSelect.Checked)
                    {
                        //  if (HasDCStoreId(Convert.ToInt32(loadGridView.DataKeys[i][0].ToString())))
                        {



                            dataRow = aDataTable.NewRow();

                            dataRow["EmpInfoId"] ="0";

                            dataRow["EmpMasterCode"] = null;
                            dataRow["EmpName"] = BM_Name.Text;
                            dataRow["Designation"] = "";

                            dataRow["Seq_No"] = "";
                            dataRow["IsMinimumApprovalPerson"] = "";
                            dataRow["CanEdit"] = "";
                            dataRow["NotificationEmail"] = "";
                            dataRow["NotificationSMS"] = "";


                            aDataTable.Rows.Add(dataRow);
                        }
                    }
                }
                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {


                    HiddenField ShfEmpInfoId = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("ApphfEmpInfoId"));
                    HiddenField hfSeq_No = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfSeq_No"));
                    Label Slbl_EmpMasterCode = (Label)gv_ApprovalPathDetail.Rows[i].FindControl("Appbl_EmpMasterCode");
                    Label Slbl_EmpName = (Label)gv_ApprovalPathDetail.Rows[i].FindControl("Applbl_EmpName");
                    Label Slbl_Designation = (Label)gv_ApprovalPathDetail.Rows[i].FindControl("Applbl_Designation");



                    HiddenField hfIsMinimumApprovalPerson = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfIsMinimumApprovalPerson"));
                    HiddenField hfCanEdit = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfCanEdit"));
                    HiddenField HiNotificationEmailApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("HiNotificationEmailApp"));
                    HiddenField hfNotificationSMSApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfNotificationSMSApp"));



                    dataRow = aDataTable.NewRow();
                    dataRow["EmpInfoId"] = ShfEmpInfoId.Value;

                    dataRow["EmpMasterCode"] = Slbl_EmpMasterCode.Text;
                    dataRow["EmpName"] = Slbl_EmpName.Text;
                    dataRow["Designation"] = Slbl_Designation.Text;
                    dataRow["Seq_No"] = hfSeq_No.Value;


                    dataRow["IsMinimumApprovalPerson"] = hfIsMinimumApprovalPerson.Value;
                    dataRow["CanEdit"] = hfCanEdit.Value;
                    dataRow["NotificationEmail"] = HiNotificationEmailApp.Value;
                    dataRow["NotificationSMS"] = hfNotificationSMSApp.Value;







                    aDataTable.Rows.Add(dataRow);
                }
                ViewState["gv_Details_App"] = aDataTable;
                gv_ApprovalPathDetail.DataSource = aDataTable;
                gv_ApprovalPathDetail.DataBind();

                if (gv_ApprovalPathDetail.Rows.Count == 0)
                {
                    lblstatus.Text = "No Approval Path have been  selected.";
                }
                else
                {
                    lblstatus.Text = "";
                }


                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {
                    DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].FindControl("ddlSequenceList");


                    ddlSequenceList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select One.....", String.Empty));
                    for (int k = 1; k < gv_ApprovalPathDetail.Rows.Count + 1; k++)
                    {
                        ddlSequenceList.Items.Insert(k, new ListItem(k.ToString()));
                    }
                }

                for (int i = 0; i < gv_ApprovalPathDetail.Rows.Count; i++)
                {
                    DropDownList ddlSequenceList = (DropDownList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("ddlSequenceList");
                    HiddenField hfSeq_No = (HiddenField)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("hfSeq_No");

                    ddlSequenceList.SelectedValue = hfSeq_No.Value;




                    HiddenField hfIsMinimumApprovalPerson = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfIsMinimumApprovalPerson"));
                    HiddenField hfCanEdit = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfCanEdit"));
                    HiddenField HiNotificationEmailApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("HiNotificationEmailApp"));
                    HiddenField hfNotificationSMSApp = ((HiddenField)gv_ApprovalPathDetail.Rows[i].FindControl("hfNotificationSMSApp"));


                    CheckBox chkMimimumCount = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkMimimumCount");

                    if (hfIsMinimumApprovalPerson.Value != "")
                    {
                        chkMimimumCount.Checked = true;

                    }


                    CheckBox chkIsEdit = (CheckBox)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkIsEdit");

                    if (hfCanEdit.Value != "")
                    {
                        chkIsEdit.Checked = true;

                    }
                    CheckBoxList chkNotificationApp = (CheckBoxList)gv_ApprovalPathDetail.Rows[i].Cells[0].FindControl("chkNotificationApp");


                    if (HiNotificationEmailApp.Value != "")
                    {
                        chkNotificationApp.Items[0].Selected = true;

                    }

                    if (hfNotificationSMSApp.Value != "")
                    {
                        chkNotificationApp.Items[1].Selected = true;

                    }

                }
            }
            else
            {
                AlertMessageBoxShow("Already Exist !!!");
            }
        }
    }

    protected void chkBSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var chkBoxHeader = (CheckBox)gv_loadGridView.HeaderRow.FindControl("chkBSelectAll");

        for (int i = 0; i < gv_loadGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)gv_loadGridView.Rows[i].Cells[0].FindControl("BchkSelect");
            chkBoxRows.Checked = chkBoxHeader.Checked;
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#exampleModalBoardMember').modal('show')", true);
    }

    protected void chkBoardMemberPosition_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((RadioButtonList)sender).Parent.Parent)).RowIndex;

        RadioButtonList chkBoardMemberPosition = ((RadioButtonList)gv_BoardMember.Rows[rowIndex].FindControl("chkBoardMemberPosition"));

        HiddenField hfBoardMemberPosition = ((HiddenField)gv_BoardMember.Rows[rowIndex].FindControl("hfBoardMemberPosition"));

        hfBoardMemberPosition.Value = chkBoardMemberPosition.SelectedValue;
    }

    protected void btnBoardMember_DetailsRemove_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["gv_BoardMember_List"] != null)
        {
            DataTable dt = (DataTable)ViewState["gv_BoardMember_List"];

            // Detect actual column name (DB data uses "PositionId", add-handler data uses "Position")
            string posColName = dt.Columns.Contains("Position") ? "Position" :
                                dt.Columns.Contains("PositionId") ? "PositionId" : null;

            for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
            {
                TextBox txtBoardMember_EmpName = (TextBox)gv_BoardMember.Rows[i].FindControl("txtBoardMember_EmpName");
                TextBox txtBoardMember_Designation = (TextBox)gv_BoardMember.Rows[i].FindControl("txtBoardMember_Designation");
                DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");

                if (txtBoardMember_EmpName != null && dt.Columns.Contains("EmpName"))
                    dt.Rows[i]["EmpName"] = txtBoardMember_EmpName.Text;
                if (txtBoardMember_Designation != null && dt.Columns.Contains("Designation"))
                    dt.Rows[i]["Designation"] = txtBoardMember_Designation.Text;
                if (ddlPosition != null && posColName != null)
                    dt.Rows[i][posColName] = ddlPosition.SelectedValue;
            }

            dt.Rows.Remove(dt.Rows[rowID]);
            // AcceptChanges resets RowState to Unchanged — prevents ViewState from
            // accumulating modified-row metadata across postbacks (reduces ViewState size).
            dt.AcceptChanges();

            if (dt.Rows.Count > 0)
            {
                ViewState["gv_BoardMember_List"] = dt;
                gv_BoardMember.DataSource = dt;
                gv_BoardMember.DataBind();
            }
            else
            {
                ViewState["gv_BoardMember_List"] = null;
                gv_BoardMember.DataSource = null;
                gv_BoardMember.DataBind();
            }
        }

        // Use session-cached position list — avoids DB hit on every remove click
        DataTable dtMemberPostion = MemberPositionCache;
        DataTable jobCreationInfos = (DataTable)ViewState["gv_BoardMember_List"];

        // Detect column name from the updated DataTable
        string posCol2 = jobCreationInfos != null && jobCreationInfos.Columns.Contains("Position") ? "Position" :
                         jobCreationInfos != null && jobCreationInfos.Columns.Contains("PositionId") ? "PositionId" : null;

        for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
        {
            DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
            if (ddlPosition == null) continue;

            ddlPosition.DataSource = dtMemberPostion;
            ddlPosition.DataValueField = "Value";
            ddlPosition.DataTextField = "TextField";
            ddlPosition.DataBind();

            if (posCol2 != null && jobCreationInfos != null && i < jobCreationInfos.Rows.Count)
            {
                try { ddlPosition.SelectedValue = jobCreationInfos.Rows[i][posCol2].ToString(); }
                catch { }
            }
        }
    }

    protected void btnBoardMember_DetailsAdd_OnClick(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;

        DataTable aTable = new DataTable();
        aTable.Columns.Add("EmpName");
        aTable.Columns.Add("Designation");
        aTable.Columns.Add("PositionId"); // use PositionId to match DB column name
        aTable.Columns.Add("BMemberSetupDetailsID");

        DataRow dr;

        for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
        {
            dr = aTable.NewRow();
            TextBox txtBoardMember_EmpName = (TextBox)gv_BoardMember.Rows[i].FindControl("txtBoardMember_EmpName");
            TextBox txtBoardMember_Designation = (TextBox)gv_BoardMember.Rows[i].FindControl("txtBoardMember_Designation");
            DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
            HiddenField hfBMemberSetupDetailsIDb = (HiddenField)gv_BoardMember.Rows[i].FindControl("hfBMemberSetupDetailsIDb");

            dr["EmpName"] = txtBoardMember_EmpName != null ? txtBoardMember_EmpName.Text : "";
            dr["Designation"] = txtBoardMember_Designation != null ? txtBoardMember_Designation.Text : "";
            dr["PositionId"] = ddlPosition != null ? ddlPosition.SelectedValue : "";
            dr["BMemberSetupDetailsID"] = hfBMemberSetupDetailsIDb != null ? hfBMemberSetupDetailsIDb.Value : "";

            aTable.Rows.Add(dr);

            if (rowIndex == i)
            {
                dr = aTable.NewRow();
                dr["EmpName"] = "";
                dr["Designation"] = "";
                dr["PositionId"] = "";
                dr["BMemberSetupDetailsID"] = "";
                aTable.Rows.Add(dr);
            }
        }

        gv_BoardMember.DataSource = aTable;
        ViewState["gv_BoardMember_List"] = aTable;
        gv_BoardMember.DataBind();

        // Use session-cached position list — avoids DB hit on every add click
        DataTable dtMemberPostion = MemberPositionCache;
        for (int i = 0; i < gv_BoardMember.Rows.Count; i++)
        {
            DropDownList ddlPosition = (DropDownList)gv_BoardMember.Rows[i].FindControl("ddlPosition");
            if (ddlPosition != null)
            {
                ddlPosition.DataSource = dtMemberPostion;
                ddlPosition.DataValueField = "Value";
                ddlPosition.DataTextField = "TextField";
                ddlPosition.DataBind();

                if (i < aTable.Rows.Count)
                {
                    try { ddlPosition.SelectedValue = aTable.Rows[i]["PositionId"].ToString(); }
                    catch { }
                }
            }
        }
    }
    protected void DropDownList2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void gv_Details_Save_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadioButtonList ddlCompanySave = (RadioButtonList)e.Row.FindControl("ddlCompanySave");
            HiddenField hfCompanySave = (HiddenField)e.Row.FindControl("hfCompanySave");
            if (ddlCompanySave != null && ddlCompanySave.Items.Count == 0)
            {
                // Use session-cached company list — avoids DB hit on every grid rebind
                ddlCompanySave.DataSource = CompaniesCache;
                ddlCompanySave.DataValueField = "Value";
                ddlCompanySave.DataTextField = "TextField";
                ddlCompanySave.DataBind();

                ListItem selectItem = ddlCompanySave.Items.FindByText("Select...");
                if (selectItem != null)
                    ddlCompanySave.Items.Remove(selectItem);

                if (hfCompanySave != null && !string.IsNullOrEmpty(hfCompanySave.Value))
                {
                    try { ddlCompanySave.SelectedValue = hfCompanySave.Value; } catch { }

                    DropDownList ddlEmployeeSaveRow = (DropDownList)e.Row.FindControl("ddlEmployeeSave");
                    if (ddlEmployeeSaveRow != null)
                    {
                        // Use session-cached employee list — avoids DB hit per row
                        ddlEmployeeSaveRow.DataSource = GetEmployeesForCompany(hfCompanySave.Value);
                        ddlEmployeeSaveRow.DataValueField = "Value";
                        ddlEmployeeSaveRow.DataTextField = "TextField";
                        ddlEmployeeSaveRow.DataBind();
                        ddlEmployeeSaveRow.Items.Insert(0, new ListItem("Select...", ""));

                        HiddenField ShfEmpInfoId = (HiddenField)e.Row.FindControl("ShfEmpInfoId");
                        if (ShfEmpInfoId != null && !string.IsNullOrEmpty(ShfEmpInfoId.Value))
                        {
                            try { ddlEmployeeSaveRow.SelectedValue = ShfEmpInfoId.Value; } catch { }
                        }
                    }
                }
            }
            
            RadioButtonList chkPosition = (RadioButtonList)e.Row.FindControl("chkPosition");
            HiddenField hfPosition = (HiddenField)e.Row.FindControl("hfPosition");
            if (chkPosition != null && hfPosition != null && !string.IsNullOrEmpty(hfPosition.Value))
            {
                try { chkPosition.SelectedValue = hfPosition.Value; } catch { }
            }

            CheckBoxList chkNotification = (CheckBoxList)e.Row.FindControl("chkNotification");
            HiddenField HiNotificationEmail = (HiddenField)e.Row.FindControl("HiNotificationEmail");
            HiddenField hfNotificationSMS = (HiddenField)e.Row.FindControl("hfNotificationSMS");
            if (chkNotification != null && chkNotification.Items.Count > 1)
            {
                if (HiNotificationEmail != null && !string.IsNullOrEmpty(HiNotificationEmail.Value))
                {
                    try { chkNotification.Items[0].Selected = Convert.ToBoolean(HiNotificationEmail.Value); } catch { }
                }
                if (hfNotificationSMS != null && !string.IsNullOrEmpty(hfNotificationSMS.Value))
                {
                    try { chkNotification.Items[1].Selected = Convert.ToBoolean(hfNotificationSMS.Value); } catch { }
                }
            }
            
            RadioButtonList rbType = (RadioButtonList)e.Row.FindControl("rbType");
            HiddenField hfType = (HiddenField)e.Row.FindControl("hfType");
            if (rbType != null && hfType != null && !string.IsNullOrEmpty(hfType.Value))
            {
                try { rbType.SelectedValue = hfType.Value.Trim(); } catch { }
            }
            
            DropDownList ddlEmployeeSave = (DropDownList)e.Row.FindControl("ddlEmployeeSave");
            TextBox txt_EmpName = (TextBox)e.Row.FindControl("txt_EmpName");

            if (rbType != null && rbType.SelectedValue == "Guest")
            {
                if (ddlCompanySave != null)
                {
                    ddlCompanySave.ClearSelection();
                    ddlCompanySave.Enabled = false;
                }
                if (ddlEmployeeSave != null) ddlEmployeeSave.Visible = false;
                if (txt_EmpName != null) txt_EmpName.Visible = true;
            }
            else
            {
                if (ddlCompanySave != null) ddlCompanySave.Enabled = true;
                if (ddlEmployeeSave != null) ddlEmployeeSave.Visible = true;
                if (txt_EmpName != null) txt_EmpName.Visible = false;
            }
        }
    }

    protected void ddlCompanySave_SelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((RadioButtonList)sender).Parent.Parent)).RowIndex;
        RadioButtonList ddlCompanySave = ((RadioButtonList)gv_Details_Save.Rows[rowIndex].FindControl("ddlCompanySave"));
        DropDownList ddlEmployeeSave = ((DropDownList)gv_Details_Save.Rows[rowIndex].FindControl("ddlEmployeeSave"));

        if (ddlCompanySave != null && ddlCompanySave.SelectedValue != "" && ddlEmployeeSave != null)
        {
            // Store in session cache so subsequent add/remove rows reuse the data
            string key = "__MtgEntry_Emps_" + ddlCompanySave.SelectedValue;
            if (Session[key] == null)
                Session[key] = AMAsterDal.GetDDLEmpInfo(ddlCompanySave.SelectedValue);

            ddlEmployeeSave.DataSource = (DataTable)Session[key];
            ddlEmployeeSave.DataValueField = "Value";
            ddlEmployeeSave.DataTextField = "TextField";
            ddlEmployeeSave.DataBind();
            ddlEmployeeSave.Items.Insert(0, new ListItem("Select...", ""));
        }
    }

    protected void ddlEmployeeSave_SelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((DropDownList)sender).Parent.Parent)).RowIndex;
        DropDownList ddlEmployeeSave = ((DropDownList)gv_Details_Save.Rows[rowIndex].FindControl("ddlEmployeeSave"));
        TextBox txt_EmpName = ((TextBox)gv_Details_Save.Rows[rowIndex].FindControl("txt_EmpName"));

        if (ddlEmployeeSave.SelectedValue != "")
        {
            txt_EmpName.Text = ddlEmployeeSave.SelectedItem.Text;
        }
    }

    protected void ddlCompanyLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompanyLocation.SelectedValue != "")
        {
            CommonDataLoadDAL commonDataLoad = new CommonDataLoadDAL();
            using (DataTable dt = commonDataLoad.GetDDLSalaryLocationByCompany(ddlCompanyLocation.SelectedValue))
            {
                ddlOffice.DataSource = dt;
                ddlOffice.DataValueField = "Value";
                ddlOffice.DataTextField = "TextField";
                ddlOffice.DataBind();
                ddlOffice.Items.Insert(0, new ListItem("Select...", ""));
            }
        }
    }
}
