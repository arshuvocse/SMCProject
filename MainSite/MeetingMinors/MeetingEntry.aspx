<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MainMasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="MeetingEntry.aspx.cs" Inherits="MeetingMinors_MeetingEntry" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js" integrity="sha512-rMGGF4wg1R73ehtnxXBt5mbUfN9JUJwbk21KMlnLZDJh7BkPmeovBuddZCENJddHYYMkCh9hPFnPmS9sspki8g==" crossorigin="anonymous"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" integrity="sha512-yVvxUQV0QESBt1SyZbNJMAwyKvFTLMyXSyBHDO4BG5t7k/Lw34tyqlSDlKIrIENIzCl+RVUNjmCPG+V/GMesRw==" crossorigin="anonymous" />
    
    <!-- Flatpickr for Time Selection -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <style>
        div#cpFormBody_ddlDepartment_chosen {
            width: 300px !important;
        }

        .Label_Title {
            background-color: #C7C7C7;
            width: 100%;
            text-align: center;
            margin: 0px;
            padding: 5px;
            text-align: center;
            color: #000;
            margin-right: 5%;
            font-weight: bold;
            font-size: 13px;
        }

        .sadasd {
            font-size: 18px;
            color:;
        }

        fieldset.for-panel {
            background-color: #fcfcfc;
            border: 1px solid #999;
            padding: 15px 10px;
            background-color: white;
            margin-bottom: 12px;
        }

        .buttonFinish {
            display: none !important;
        }

        .wizard .buttonNext.meeting-next-disabled {
            opacity: 0.55;
            cursor: not-allowed !important;
            pointer-events: none;
        }

        /* Meeting and Minutes are now presented as one entry form. */
        .meeting-entry-form > ul {
            display: none !important;
        }

        .meeting-entry-form #step-7,
        .meeting-entry-form #step-9 {
            display: block !important;
            width: 100% !important;
        }

        .meeting-entry-form #step-8,
        .meeting-entry-form #step-10 {
            display: none !important;
        }

        .member-search-actions {
            display: flex;
            justify-content: flex-end;
            margin: 4px 0 8px;
        }

        .member-search-scroll,
        .draft-member-scroll {
            max-height: 646px;
            overflow-y: auto;
            overflow-x: auto;
            border: 1px solid #d6e2ea;
            position: relative;
        }

        .member-search-scroll table,
        .draft-member-scroll table {
            margin-bottom: 0 !important;
            border-collapse: separate;
            border-spacing: 0;
        }

        .member-search-scroll thead th,
        .draft-member-scroll thead th {
            position: sticky;
            top: 0;
            z-index: 10;
            background: linear-gradient(to bottom, #4298c4 0%, #247ca9 100%) !important;
            color: #fff !important;
            box-shadow: 0 2px 0 rgba(20, 84, 116, 0.35);
        }

        .member-search-scroll tbody tr,
        .draft-member-scroll tbody tr {
            height: 40px;
        }

        .member-search-scroll td,
        .draft-member-scroll td {
            padding-top: 5px !important;
            padding-bottom: 5px !important;
            vertical-align: middle !important;
        }

        .draft-member-scroll .btn {
            padding: 4px 10px !important;
        }

        .draft-member-panel {
            margin-top: 18px;
            padding-top: 12px;
            border-top: 1px solid #d9e2e8;
        }

        fieldset.for-panel legend {
            background-color: #fafafa;
            border: 1px solid #ddd;
            border-radius: 1px;
            font-size: 12px;
            font-weight: bold;
            line-height: 10px;
            margin: inherit;
            padding: 7px;
            width: auto;
            margin-bottom: 0;
            color: black;
        }


        .chkChoice label {
            padding-left: 4px;
            padding-right: 4px;
        }

        .modal-dialog2 {
            max-width: 90% !important;
            margin-left: 14%;
            margin-top: 1.75rem;
        }

        div#cpFormBody_ddlEmp_chosen {
            width: 100% !important;
        }

        .title-widget {
            color: #898989;
            font-size: 20px;
            font-weight: 300;
            line-height: 1;
            position: relative;
            text-transform: uppercase;
            font-family: 'Fjalla One', sans-serif;
            margin-top: 0;
            margin-right: 0;
            margin-bottom: 25px;
            padding-left: 12px;
        }


            .title-widget::before {
                background-color: #ea5644;
                content: "";
                height: 22px;
                left: 0px;
                position: absolute;
                top: -2px;
                width: 5px;
            }

        /* Premium UI Modernization */
        .title-widget {
            color: #1e293b !important;
            font-size: 16px !important;
            font-weight: 700 !important;
            letter-spacing: 0.5px;
            text-transform: uppercase;
            border-left: 4px solid #5B799E !important;
            padding-left: 12px;
            margin-bottom: 20px;
            font-family: inherit !important;
            text-shadow: none !important;
        }
        .title-widget::before {
            display: none !important;
        }
        
        .meeting-entry-form .form-control {
            border-radius: 6px !important;
            border: 1px solid #cbd5e1 !important;
            font-size: 13.5px !important;
            padding: 6px 12px !important;
            transition: all 0.2s ease !important;
            height: 38px !important;
        }

        .meeting-entry-form textarea.form-control {
            height: auto !important;
        }

        .meeting-entry-form .form-control:focus {
            border-color: #5B799E !important;
            box-shadow: 0 0 0 3px rgba(91, 121, 158, 0.15) !important;
        }

        .meeting-entry-form .control-label {
            font-size: 13px !important;
            font-weight: 600 !important;
            color: #334155 !important;
        }

        .card {
            border: 1px solid #e2e8f0 !important;
            border-radius: 12px !important;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05), 0 2px 4px -1px rgba(0, 0, 0, 0.03) !important;
        }

        fieldset.for-panel {
            border: 1px solid #e2e8f0 !important;
            border-radius: 10px !important;
            background-color: #f8fafc !important;
            padding: 20px !important;
            margin-bottom: 20px !important;
        }

        fieldset.for-panel legend {
            font-size: 13px !important;
            font-weight: 700 !important;
            color: #5B799E !important;
            background-color: #ffffff !important;
            border: 1px solid #e2e8f0 !important;
            border-radius: 6px !important;
            padding: 6px 14px !important;
        }

        /* GridView Styling */
        .blueTableNew, .AddToListCssTable {
            border-collapse: separate !important;
            border-spacing: 0 !important;
            width: 100% !important;
            border: 1px solid #e2e8f0 !important;
            border-radius: 8px !important;
            overflow: visible !important;
            margin-top: 10px;
            margin-bottom: 15px;
            box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.05) !important;
        }

        .blueTableNew th, .AddToListCssTable th {
            background: #5B799E !important;
            color: #ffffff !important;
            font-weight: 600 !important;
            font-size: 13px !important;
            padding: 12px 16px !important;
            border-bottom: 2px solid #475e7a !important;
            border-top: none !important;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            vertical-align: middle !important;
        }

        .blueTableNew td, .AddToListCssTable td {
            padding: 10px 16px !important;
            font-size: 13px !important;
            color: #334155 !important;
            border-bottom: 1px solid #e2e8f0 !important;
            border-top: none !important;
            background-color: #ffffff;
            vertical-align: middle !important;
            transition: background-color 0.2s ease;
        }

        .blueTableNew tr:hover td, .AddToListCssTable tr:hover td {
            background-color: #f8fafc !important;
        }

        .blueTableNew tr:last-child td, .AddToListCssTable tr:last-child td {
            border-bottom: none !important;
        }

        .chkChoice {
            display: inline-table;
            border: none !important;
        }

        .chkChoice td {
            padding: 2px 8px !important;
            border: none !important;
            background: transparent !important;
        }

        .chkChoice label {
            font-size: 13px !important;
            color: #475569 !important;
            font-weight: 500 !important;
            cursor: pointer;
            margin-bottom: 0 !important;
            padding-left: 6px !important;
        }

        .chkChoice input[type="radio"], .chkChoice input[type="checkbox"] {
            margin-top: 2px;
            cursor: pointer;
            transform: scale(1.1);
        }

        .blueTableNew .form-control {
            border-radius: 6px !important;
            border: 1px solid #cbd5e1 !important;
            height: 32px !important;
            font-size: 13px !important;
            padding: 4px 8px !important;
            transition: all 0.2s ease;
        }

        .blueTableNew .form-control:focus {
            border-color: #5B799E !important;
            box-shadow: 0 0 0 3px rgba(91, 121, 158, 0.15) !important;
        }

        .blueTableNew .btn {
            border-radius: 6px !important;
            padding: 4px 8px !important;
            font-size: 12px !important;
            margin: 0 2px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpFormBody" runat="Server">

    <div class="modal fade" id="exampleModal2" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog  modal-lg" style="width: 90% !important;" role="document">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                        <ProgressTemplate>
                            <div class="divWaiting">
                                <asp:Image ID="imgWait7" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div class="modal-content">
                        <div class="modal-header">



                            <h3 class="modal-title" id="exampleModalLabel2" style="color: #2196F3; text-shadow: 0 0 1px black;">Add More Member</h3>




                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-12">

                                    <h2 class="blue title-widget" style="color: #2196F3; text-shadow: 0 0 2px black;">Search Member</h2>

                                    <div class="row">
                                        <div class="col-md-2" style="padding-top: 8px">
                                            <label class="control-label">Company:</label>
                                        </div>
                                        <div class="col-md-3">

                                            <asp:DropDownList runat="server" ID="ddlComSearch" AutoPostBack="True" OnSelectedIndexChanged="ddlComSearch_OnSelectedIndexChanged" class="form-control form-control-sm" />
                                            <script type="text/javascript">
                                                // Shared so client-side-only re-renders (Grid B's Add/Remove row
                                                // buttons, which don't cause a postback) can re-apply Chosen too —
                                                // not just pageLoad(), which only fires after an actual postback.
                                                function refreshChosenWidgets() {
                                                    var $chosenSelects = $(
                                                        '#<%=ddlComSearch.ClientID%>, ' +
                                                        '#<%=ddlDivision.ClientID%>, ' +
                                                        '#<%=ddlDepartment.ClientID%>, ' +
                                                        '#<%=ddlEmp.ClientID%>, ' +
                                                        '#<%=ddlDivisionAPP.ClientID%>, ' +
                                                        '#<%=ddlDepartmentAPP.ClientID%>, ' +
                                                        '#<%=ddlCompanyLocation.ClientID%>, ' +
                                                        '#<%=ddlOffice.ClientID%>, ' +
                                                        '#<%=ddlLocation.ClientID%>, ' +
                                                        '#<%=ddlFloor.ClientID%>, ' +
                                                        '#<%=ddlMettingRoomName.ClientID%>, ' +
                                                        '.SelectMe33'
                                                    );

                                                    $chosenSelects.each(function () {
                                                        var $select = $(this);

                                                        if ($select.data('chosen')) {
                                                            $select.chosen('destroy');
                                                        }

                                                        $select.chosen({
                                                            disable_search_threshold: 5,
                                                            search_contains: true,
                                                            width: '100%'
                                                        });
                                                    });
                                                }

                                                function pageLoad() {
                                                    // Repaint the client-driven Add-Employees / Members-List tables
                                                    // from their in-memory row arrays. This runs after the initial
                                                    // load AND after every async postback (MS AJAX convention), which
                                                    // is what lets these tables survive unrelated UpdatePanel refreshes
                                                    // that would otherwise wipe their <tbody> markup.
                                                    if (window.MeetingGridA) MeetingGridA.render();
                                                    if (window.MeetingGridB) MeetingGridB.render();

                                                    refreshChosenWidgets();

                                                    // Initialize flatpickr for time inputs
                                                    $('.flatpickr-time').flatpickr({
                                                        enableTime: true,
                                                        noCalendar: true,
                                                        dateFormat: "H:i", // Sent to server
                                                        altInput: true,
                                                        altFormat: "h:i K", // Shown to user (AM/PM)
                                                        time_24hr: false // User-friendly AM/PM in the picker
                                                    });
                                                }
                                            </script>
                                        </div>



                                        <div class="col-md-2" style="padding-top: 8px">
                                            <label class="control-label">Division:</label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList runat="server" ID="ddlDivision" OnSelectedIndexChanged="ddlDivision_OnSelectedIndexChanged" AutoPostBack="True" class="form-control form-control-sm"></asp:DropDownList>



                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">



                                        <div class="col-md-2" style="padding-top: 8px">
                                            <label class="control-label ">Department:</label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList runat="server" ID="ddlDepartment" class="form-control form-control-sm"></asp:DropDownList>
                                        </div>


                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-3" style="padding-top: 8px">
                                            <label class="control-label">Employee Name:</label>
                                        </div>
                                        <div class="col-md-5">

                                            <asp:DropDownList runat="server" ID="ddlEmp" Style="width: 100% !important" class="form-control form-control-sm SelectMe33" />
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton runat="server" ID="btnSearch" CssClass="btn btn-success   btn-sm" OnClick="btnSearch_OnClick"><span aria-hidden="true" class="fa fa-search-plus"></span>  &nbsp; Search </asp:LinkButton>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="member-search-actions">
                                        <asp:LinkButton runat="server" ID="btnAddToListEmp" CssClass="btn btn-success btn-sm"
                                            OnClientClick="prepareGridAExistingCodes();"
                                            OnClick="btnStageMembers_OnClick"><i class="fa fa-plus-circle"></i>&nbsp; Add To List</asp:LinkButton>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="member-search-scroll">
                                            <asp:GridView Width="100%" ShowHeader="True" ID="gv_EmpListSearch" runat="server" AutoGenerateColumns="false" CssClass="AddToListCssTable" OnPreRender="gv_DocumentUpload_PreRender">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SL#">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="form-control-sm" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" CssClass="form-control-sm" runat="server" />

                                                            <asp:HiddenField runat="server" ID="hfEmpInfoId" Value='<%#Eval("EmpInfoId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Employee ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_EmpMasterCode" runat="server" Text='<%#Eval("EmpMasterCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Employee Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_EmpName" runat="server" Text='<%#Eval("EmpName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Designation">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Designation" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="draft-member-panel">
                                        <h4 style="color: #1688b1;">Selected Members (Draft)</h4>
                                        <div class="draft-member-scroll">
                                        <asp:GridView Width="100%" ShowHeader="True" ID="gv_DraftMemberList" runat="server"
                                            AutoGenerateColumns="false" CssClass="AddToListCssTable" OnPreRender="gv_DocumentUpload_PreRender">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL#">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee ID">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hfDraftEmpInfoId" Value='<%#Eval("EmpInfoId")%>' />
                                                        <asp:Label runat="server" ID="lblDraftEmpMasterCode" Text='<%#Eval("EmpMasterCode")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Name">
                                                    <ItemTemplate><asp:Label runat="server" ID="lblDraftEmpName" Text='<%#Eval("EmpName")%>' /></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation">
                                                    <ItemTemplate><asp:Label runat="server" ID="lblDraftDesignation" Text='<%#Eval("Designation")%>' /></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="btnRemoveDraftMember" CssClass="btn btn-danger btn-sm"
                                                            OnClick="btnRemoveDraftMember_OnClick"><i class="fa fa-times"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        </div>

                                        <div class="text-right" style="margin-top: 12px;">
                                            <asp:LinkButton runat="server" ID="btnSubmitDraftMembers" CssClass="btn btn-info btn-sm"
                                                OnClientClick="prepareGridAExistingCodes();"
                                                OnClick="btnSubmitDraftMembers_OnClick"><i class="fa fa-check"></i>&nbsp; Submit</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>






                            </div>


                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div class="modal fade" id="exampleModalAPPEmpp" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog  modal-lg" style="width: 90% !important;" role="document">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                        <ProgressTemplate>
                            <div class="divWaiting">
                                <asp:Image ID="imgWait8" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div class="modal-content">
                        <div class="modal-header">



                            <h3 class="modal-title" id="exampleModalLabelApp" style="color: #2196F3; text-shadow: 0 0 1px black;">Add More Member in Approval Path</h3>





                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-12">

                                    <h2 class="blue title-widget" style="color: #2196F3; text-shadow: 0 0 2px black;">Search Member</h2>

                                    <div class="row">


                                        <div class="col-md-1" style="padding-top: 8px">
                                            <label class="control-label">Division:</label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList runat="server" ID="ddlDivisionAPP" OnSelectedIndexChanged="ddlDivisionAPP_OnSelectedIndexChanged" AutoPostBack="True" class="form-control form-control-sm"></asp:DropDownList>

                                            <%--<script type="text/javascript">
                                          function pageLoad() {
                                              $('#<%=ddlDivision.ClientID%>').chosen({ disable_search_threshold: 5, search_contains: true });

                         $('#<%=ddlDepartment.ClientID%>').chosen({ disable_search_threshold: 5, search_contains: true });
                     }
               </script>--%>
                                        </div>

                                        <div class="col-md-2" style="padding-top: 8px">
                                            <label class="control-label ">Department:</label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList runat="server" ID="ddlDepartmentAPP" class="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton runat="server" ID="btnSearchAPP" CssClass="btn btn-success   btn-sm" OnClick="btnSearchAPP_OnClick"><span aria-hidden="true" class="fa fa-search-plus"></span>  &nbsp; Search </asp:LinkButton>
                                        </div>

                                    </div>
                                    <br />

                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:GridView Width="100%" ShowHeader="True" ID="gv_EmpListSearchAPP" runat="server" AutoGenerateColumns="false" CssClass="AddToListCssTable" OnPreRender="gv_DocumentUpload_PreRender">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SL#">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAllAPP" runat="server" CssClass="form-control-sm" AutoPostBack="True" OnCheckedChanged="chkSelectAllAPP_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectAPP" CssClass="form-control-sm" runat="server" />

                                                            <asp:HiddenField runat="server" ID="hfEmpInfoIdAPP" Value='<%#Eval("EmpInfoId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Employee ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_EmpMasterCodeAPP" runat="server" Text='<%#Eval("EmpMasterCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Employee Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_EmpNameAPP" runat="server" Text='<%#Eval("EmpName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Designation">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_DesignationAPP" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>



                                    <br />
                                    <div class="row">
                                        <div class="col-4"></div>
                                        <div class="col-4"></div>
                                        <div class="col-4">
                                            <asp:Button runat="server" CssClass="btn btn-outline-success btn-block disabled btn-sm" ID="btnAddToListEmpAPP" OnClick="btnAddToListEmpAPP_OnClick" Text="Add To List" />
                                        </div>
                                    </div>
                                </div>






                            </div>


                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div class="modal fade" id="exampleModalAPP" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog  modal-lg" role="document">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                        <ProgressTemplate>
                            <div class="divWaiting">
                                <asp:Image ID="imgWait9" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div class="modal-content">
                        <div class="modal-header">



                            <h3 class="modal-title" id="exampleModalLabels" style="color: #2196F3; text-shadow: 0 0 1px black;">Routing Path</h3>




                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-2">
                                    <div class="form-group">
                                    </div>

                                </div>


                                <div class="col-md-12">

                                    <div class="Label_Title">Routing Path List</div>

                                    <br />
                                    <div class="form-group">
                                        <div style="overflow: scroll; height: 230px">
                                            <asp:RadioButtonList ID="rbRoutingPath" CssClass="chkChoice" RepeatColumns="5" RepeatDirection="Horizontal" runat="server" />
                                        </div>




                                    </div>


                                </div>



                            </div>


                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton runat="server" class="btn btn-success" OnClick="btnOkayRoutingPath_OnClick" ID="btnOkayRoutingPath">Okay</asp:LinkButton>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>




    <div class="modal fade" id="exampleModalBoardMember" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog  modal-lg" role="document">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress11" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                        <ProgressTemplate>
                            <div class="divWaiting">
                                <asp:Image ID="imgWait9ss" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div class="modal-content">
                        <div class="modal-header">



                            <h3 class="modal-title" id="exampleModalLabels" style="color: #2196F3; text-shadow: 0 0 1px black;">Add Board-Member</h3>




                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">



                                <div class="col-md-12">
                                    <div id="gridContainer1" style="height: auto; overflow: auto; width: auto; overflow-y: scroll; overflow-x: hidden;">
                                        <asp:GridView ID="gv_loadGridView" runat="server" AutoGenerateColumns="False"
                                            CssClass="AddToListCssTable" OnPreRender="gv_DocumentUpload_PreRender" DataKeyNames="MemberSetupDetailsID">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hfMasterId" Value='<%#Eval("MemberSetupDetailsID")%>' />

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkBSelectAll" runat="server" CssClass="form-control-sm" AutoPostBack="True" OnCheckedChanged="chkBSelectAll_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="BchkSelect" CssClass="form-control-sm" runat="server" />


                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="ShortName" HeaderText="Company" />--%>
                                                <asp:BoundField DataField="MemberType" HeaderText="Member Type" />
                                                <asp:BoundField DataField="Name" HeaderText="Name" />

                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="BM_Name" runat="server" Text='<%#Eval("Name") %>'></asp:Label>


                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No." />


                                                <asp:BoundField DataField="JoiningDate" HeaderText="Joining Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="MembershipDate" HeaderText="Membership Date" DataFormatString="{0:dd-MMM-yyyy}" />





                                            </Columns>

                                        </asp:GridView>
                                    </div>



                                </div>



                            </div>
                            <br />
                            <div class="row">
                                <div class="col-4"></div>
                                <div class="col-4"></div>
                                <div class="col-4">
                                    <asp:Button runat="server" CssClass="btn btn-outline-success btn-block disabled btn-sm" ID="Button1" OnClick="AddModalBoardMember_OnClick" Text="Add To List" />
                                </div>
                            </div>


                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton Visible="False" runat="server" class="btn btn-success" OnClick="btnOkayRoutingPath_OnClick" ID="LinkButton2">Okay</asp:LinkButton>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <div class="content" id="content">
        <div class="container-fluid">
            <div class="page-heading">
                <div class="page-heading__container">

                    <h1 class="title" style="font-size: 18px; padding-top: 0px;">
                        <img src="../Report_Pages/app.png" width="20px" />
                        Meeting Entry </h1>
                </div>
                <div class="page-heading__container float-right d-none d-sm-block">
                    <asp:Button ID="detailsViewButton" Text="List Information " CssClass="btn btn-sm btn-outline-secondary " runat="server" OnClick="detailsViewButton_OnClick" />
                </div>
            </div>







            <div class="card">
                <div class="card-body">

                    <div class="row">
                        <div class="col-md-12">
                            <form action="javascript:alert('Submited!');" method="post">
                                <div class="meeting-entry-form">
                                    <ul>
                                        <li><a href="#step-7"><span class="stepNumber">1</span> <span class="stepDesc">Meeting Entry<br>
                                        </span></a></li>
                                        <li><a href="#step-8"><span class="stepNumber">2</span> <span class="stepDesc">Agenda Entry<br>
                                        </span></a></li>
                                        <li><a href="#step-9"><span class="stepNumber">3</span> <span class="stepDesc">Mintues Entry<br>
                                        </span></a></li>
                                        <li><a href="#step-10"><span class="stepNumber">4</span> <span class="stepDesc">Approval Setup<br>
                                        </span></a></li>
                                    </ul>
                                    <div id="step-7">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                    <ProgressTemplate>
                                                        <div class="divWaiting">
                                                            <asp:Image ID="imgWait11" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <div class="form-group">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group ">

                                                                <div class="row">
                                                                    <div class="col-md-3" style="padding-top: 8px">
                                                                        <label class="control-label pull-right">Company:<span style="color: red;" title="please fill out this field"> * </span></label>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged" ID="ddlCompany" class="form-control form-control-sm" onchange="handleMeetingGridsSave();" />



                                                                    </div>
                                                                </div>
                                                                <div style="padding-top: 5px;"></div>
                                                                <div class="row">
                                                                    <div class="col-md-3" style="padding-top: 8px">
                                                                        <label class="control-label pull-right">Title:<span style="color: red;" title="please fill out this field"> * </span></label>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox runat="server" ID="txtTitle" TextMode="MultiLine" Rows="2" class="form-control " />
                                                                    </div>
                                                                </div>

                                                                <div style="padding-top: 5px;"></div>
                                                                <div class="row">
                                                                    <div class="col-md-3" style="padding-top: 8px">
                                                                        <label class="control-label pull-right">
                                                                            Meeting Note
:<span style="color: red;" title="please fill out this field"> * </span>
                                                                        </label>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox runat="server" ID="txtMeetingpurpose" class="form-control" TextMode="MultiLine" Rows="3" />
                                                                    </div>
                                                                </div>

                                                                
                                                                <div style="padding-top: 5px;"></div>
                                                                 <div class="row">
                                                                    <div class="col-md-3" style="padding-top: 8px">
                                                                        <label class="control-label pull-right">
                                                                           Notice:
                                                                        </label>
                                                                    </div>
                                                                    <div class="col-md-6" style="margin-top:6px">
                                                                         
                                                                        <asp:RadioButtonList runat="server" ID="rbNotice" CssClass="chkChoice" RepeatDirection="Horizontal" >
                                                                              <asp:ListItem Selected="True" Value="1">Yes</asp:ListItem>
      <asp:ListItem Value="2">No</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </div>
                                                                </div>
                                                                <fieldset class="for-panel" runat="server" visible="False">
                                                                    <legend>Organizer
                                                                    </legend>
                                                                    <div class="row">
                                                                        <div class="col-md-3" style="padding-top: 8px">
                                                                            <label class="control-label pull-right">
                                                                                <asp:RadioButton runat="server" CssClass="chkChoice" Text="By Deparment" ID="rbDeptBy" /></label>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:DropDownList runat="server" ID="ddlDept" class="form-control form-control-sm" />
                                                                        </div>

                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-3" style="padding-top: 8px">
                                                                            <label class="control-label pull-right">
                                                                                <asp:RadioButton runat="server" Text="By Employee" CssClass="chkChoice" ID="RadioButton1" /></label>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:DropDownList runat="server" ID="DropDownList1" class="form-control form-control-sm" />
                                                                        </div>

                                                                    </div>



                                                                    <div class="row">
                                                                        <div class="col-md-3" style="padding-top: 8px">
                                                                            <label class="control-label pull-right">
                                                                                <asp:RadioButton runat="server" CssClass="chkChoice" Text="Other" ID="RadioButton2" /></label>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:TextBox runat="server" ID="txtOther" class="form-control form-control-sm" />
                                                                        </div>

                                                                    </div>
                                                                </fieldset>



                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">

                                                            <div class="row">
                                                                <div class="col-md-3" style="padding-top: 8px">
                                                                    <label class="control-label pull-right" style="white-space: nowrap;">Meeting Category:<span style="color: red;" title="please fill out this field"> * </span></label>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:DropDownList runat="server" ID="ddlCategory"  class="form-control form-control-sm">
                                                                        <%--AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"--%>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div style="padding-top: 5px;"></div>

                                                            <div class="row" runat="server" visible="False" id="DivSubCommitte">
                                                                <div class="col-md-3" style="padding-top: 8px">
                                                                    <label class="control-label pull-right" style="white-space: nowrap;">Sub-Committee Name:<span style="color: red;" title="please fill out this field"> * </span></label>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:DropDownList runat="server" ID="ddlSubCommittee" AutoPostBack="True" OnSelectedIndexChanged="ddlSubCommittee_OnSelectedIndexChanged" class="form-control form-control-sm">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div style="padding-top: 5px;"></div>

                                                            <div class="row">
                                                                <div class="col-md-3" style="padding-top: 8px">
                                                                    <label class="control-label pull-right">Meeting Date:<span style="color: red;" title="please fill out this field"> * </span></label>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:TextBox runat="server" ID="txtMeetingDate" autocomplete="off" class="form-control form-control-sm" />

                                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="TopLeft"
                                                                        Format="dd-MMM-yyyy" PopupButtonID="txtMeetingDate" CssClass="MyCalendar"
                                                                        TargetControlID="txtMeetingDate" />

                                                                </div>
                                                            </div>
                                                            <div style="padding-top: 5px;"></div>
                                                            <div class="row">
                                                                <div class="col-md-3" style="padding-top: 8px">
                                                                    <label class="control-label pull-right">Start Time:</label>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:TextBox runat="server" ID="txtStartTime" class="form-control form-control-sm flatpickr-time" placeholder="Select Start Time" />
                                                                </div>
                                                            </div>

                                                            <div style="padding-top: 5px;"></div>
                                                            <div class="row">


                                                                <div class="col-md-3" style="padding-top: 8px">
                                                                    <label class="control-label pull-right">End Time:</label>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:TextBox runat="server" ID="txtEndTime" class="form-control form-control-sm flatpickr-time" placeholder="Select End Time" />
                                                                </div>
                                                            </div>


                                                        </div>
                                                    </div>

                                                    <div style="padding-top: 15px;"></div>

                                                    <fieldset class="for-panel" style="margin-top: 15px; margin-bottom: 15px;">
                                                        <legend>Meeting Location</legend>
                                                        <div class="row mb-3">
                                                            <div class="col-md-2" style="padding-top: 8px">
                                                                <label class="control-label pull-right">Location Type:</label>
                                                            </div>
                                                            <div class="col-md-10">
                                                                <asp:RadioButtonList runat="server" AutoPostBack="True" ID="rbLocation" OnSelectedIndexChanged="rbLocation_OnSelectedIndexChanged" CssClass="chkChoice" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="Office">Office Premisis</asp:ListItem>
                                                                    <asp:ListItem Value="Outer">Outer Premisis</asp:ListItem>
                                                                    <asp:ListItem Value="Virtual">Virtual Meeting</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                             <div class="row">
                                                                 <div class="col-md-8 offset-md-2 location-panel-office">
                                                                     <h2 class="blue title-widget" style="font-size: 14px; margin-bottom: 10px;">Office Premisis</h2>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Company:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyLocation_OnSelectedIndexChanged" ID="ddlCompanyLocation" CssClass="form-control form-control-sm" />
                                                                         </div>
                                                                     </div>
                                                                     <div style="padding-top: 5px;"></div>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Office:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOffice_OnSelectedIndexChanged" ID="ddlOffice" CssClass="form-control form-control-sm" />
                                                                         </div>
                                                                     </div>
                                                                     <div style="padding-top: 5px;"></div>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Location:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:DropDownList runat="server" ID="ddlLocation" AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_OnSelectedIndexChanged" CssClass="form-control form-control-sm" />
                                                                         </div>
                                                                     </div>
                                                                     <div style="padding-top: 5px;"></div>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Floor:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFloor_OnSelectedIndexChanged" ID="ddlFloor" CssClass="form-control form-control-sm" />
                                                                         </div>
                                                                     </div>
                                                                     <div style="padding-top: 5px;"></div>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Meeting Room:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:DropDownList runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMettingRoomName_OnSelectedIndexChanged" ID="ddlMettingRoomName" CssClass="form-control form-control-sm" />
                                                                         </div>
                                                                     </div>
                                                                     <div style="padding-top: 5px;"></div>
                                                                     <div class="row" style="display: none;">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Capacity:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:TextBox runat="server" ID="txtCapacity" CssClass="form-control form-control-sm" />
                                                                         </div>
                                                                     </div>
                                                                 </div>
                                                                 <div class="col-md-8 offset-md-2 location-panel-outer">
                                                                     <h2 class="blue title-widget" style="font-size: 14px; margin-bottom: 10px;">Outer Premisis</h2>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Location:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:TextBox runat="server" ID="txtLocation" CssClass="form-control form-control-sm" />
                                                                         </div>
                                                                     </div>
                                                                     <div style="padding-top: 5px;"></div>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Description:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:TextBox runat="server" TextMode="MultiLine" Rows="2" ID="txtDescription" CssClass="form-control" />
                                                                         </div>
                                                                     </div>
                                                                 </div>
                                                                 <div class="col-md-8 offset-md-2 location-panel-virtual">
                                                                     <h2 class="blue title-widget" style="font-size: 14px; margin-bottom: 10px;">Virtual Meeting</h2>
                                                                     <div class="row">
                                                                         <div class="col-md-4" style="padding-top: 8px">
                                                                             <label class="control-label pull-right">Remarks:</label>
                                                                         </div>
                                                                         <div class="col-md-8">
                                                                             <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                                                         </div>
                                                                     </div>
                                                                 </div>
                                                             </div>
                                                    </fieldset>
                                                    <div class="row">
                                                        <h2 class="blue title-widget">Members List</h2>

                                                        <div class="col-md-12">

                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:UpdateProgress ID="UpdateProgress12" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                                        <ProgressTemplate>
                                                                            <div class="divWaiting">
                                                                                <asp:Image ID="imgWait1ee" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                    <table id="gridB_Table" width="100%" class="blueTableNew">
                                                                        <thead>
                                                                            <tr>
                                                                                <th>SL#</th>
                                                                                <th>Employee Name</th>
                                                                                <th>Designation</th>
                                                                                <th>Position</th>
                                                                                <th>Actions</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody id="gridB_Body"></tbody>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>


                                                        </div>
                                                    </div>

                                                    <br />

                                                    <div class="row">
                                                         <h2 class="blue title-widget">Add Employees</h2>

                                                         <div class="col-md-4"></div>
                                                     </div>

                                                     <div class="row">


                                                         <div class="col-md-12">

                                                             <asp:UpdatePanel runat="server">
                                                                 <ContentTemplate>
                                                                     <asp:UpdateProgress ID="UpdateProgress5" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                                         <ProgressTemplate>
                                                                             <div class="divWaiting">
                                                                                 <asp:Image ID="imgWait1" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                                                             </div>
                                                                         </ProgressTemplate>
                                                                     </asp:UpdateProgress>
                                                                                     <table id="gridA_Table" width="100%" class="blueTableNew">
                                                                                         <thead>
                                                                                             <tr>
                                                                                                 <th>SL#</th>
                                                                                                 <th>Type</th>
                                                                                                 <th>Company</th>
                                                                                                 <th>Employee Name</th>
                                                                                                 <th>Designation</th>
                                                                                                 <th>Position</th>
                                                                                                 <th>Actions</th>
                                                                                             </tr>
                                                                                         </thead>
                                                                                         <tbody id="gridA_Body"></tbody>
                                                                                     </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>


                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div id="step-8">
                                        <div class="row">


                                            <div class="col-md-12">

                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                            <ProgressTemplate>
                                                                <div class="divWaiting">
                                                                    <asp:Image ID="imgWait2" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                                                </div>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                        <asp:GridView ID="gv_AgendaList" runat="server" CssClass="blueTableNew" AutoGenerateColumns="False"
                                                            GridLines="None" OnPreRender="gv_DocumentUpload_PreRender">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="SL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Agenda">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAgenda" runat="server" Text='<%#Eval("Agenda") %>'
                                                                            class="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Observation">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtObservation" runat="server" Text='<%#Eval("Observation") %>'
                                                                            class="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Decision">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDecision" runat="server" Text='<%#Eval("Decision") %>'
                                                                            class="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Remarks">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>'
                                                                            class="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                  <asp:TemplateField HeaderText="Implementation Status">
                                                                                <ItemTemplate>

                                                                                    <asp:DropDownList runat="server" Width="150px" ID="ddlImplementationStatus" class="form-control form-control-sm " >
                                                                                           <asp:ListItem>Implemented</asp:ListItem>
                                                                                        <asp:ListItem>Ongoing</asp:ListItem>
                                                                                        <asp:ListItem>Not Implemented</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                 
                                                                        <asp:HiddenField runat="server" ID="hfImplementationStatus" Value='<%#Eval("ImplementationStatus") %>' />

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Presentor" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlPresentor" runat="server"
                                                                            class="form-control form-control-sm SelectMe">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField runat="server" ID="hfPresentor" Value='<%#Eval("Presentor") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Actions">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="btnAgenaListAdd" OnClick="btnAgenaListAdd_OnClick" CssClass="btn btn-sm btn-success"><i class="fa fa-plus"></i> </asp:LinkButton>
                                                                        <asp:LinkButton runat="server" ID="btnAgenaLisRemove" CssClass="btn btn-sm btn-danger" OnClick="btnAgenaLisRemove_OnClick"><i class="fa fa-minus-circle"></i> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>


                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>


                                            </div>
                                        </div>
                                    </div>


                                    <div id="step-9">
                                        
                                        <div class="form-group">
                                            <div class="row">

                                                <div class="col-md-12">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:UpdateProgress ID="UpdateProgress7" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                                <ProgressTemplate>
                                                                    <div class="divWaiting">
                                                                        <asp:Image ID="imgWait3" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                                                    </div>
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                            <fieldset class="for-panel">
                                                                <legend>Document </legend>
                                                                <div class="row">




                                                                    <asp:HiddenField runat="server" ID="hfDocFileName" />

                                                                    <asp:HiddenField runat="server" ID="hfDocFile" />
                                                                    <div class="col-4">
                                                                        <div class="form-group">
                                                                            <label>Document Upload<span style="color: red;" title="please fill out this field"> * </span></label>
                                                                            <div>
                                                                                <input type="file" name="postedFile" class="form-control form-control-sm" />
                                                                                <asp:FileUpload ID="FUDocument" Visible="False" CssClass="form-control form-control-sm" runat="server" />
                                                                                <br />
                                                                                <input type="button" class="btn btn-sm  btn-info" id="btnUpload" value="Upload Document" />
                                                                                <asp:LinkButton runat="server" Visible="False" OnClick="btnDocUp_OnClick" ID="btnDocUp" CssClass="btn btn-sm  btn-info">
                                                          
                                                      
              &nbsp;    <span class="btn-label"><i class="fa fa-upload"></i></span>  &nbsp;   &nbsp;Upload Document
                                                                                </asp:LinkButton>
                                                                                <br />
                                                                                <progress id="fileProgress" style="display: none"></progress>
                                                                                <br />

                                                                                <span id="lblMessage" style="color: Green"></span>
                                                                                <asp:HyperLink ID="HyperLink2" Visible="False" runat="server"
                                                                                    Target="_blank" ToolTip="Click to Show Document"></asp:HyperLink>



                                                                            </div>
                                                                        </div>
                                                                    </div>


                                                                    <div class="col-4">
                                                                        <div class="form-group">
                                                                            <label>Summary Note<span style="color: red;" title="please fill out this field"> * </span></label>
                                                                            <div>

                                                                                <asp:TextBox runat="server" ID="txtSummaryNote" TextMode="MultiLine" Rows="2" class="form-control" />

                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <asp:LinkButton runat="server" ID="brnAddDoc" OnClick="brnAddDoc_OnClick" CssClass="btn btn-success   btn-sm pull-right"><span aria-hidden="true" class="fa fa-plus"></span>  &nbsp; Add to List </asp:LinkButton>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-8">


                                                                        <asp:GridView Width="100%" ShowHeader="True" ID="gv_DocumentUpload" runat="server" AutoGenerateColumns="false" CssClass="AddToListCssTable" OnPreRender="gv_DocumentUpload_PreRender">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="SL#">
                                                                                    <ItemTemplate>
                                                                                        <%#Container.DataItemIndex + 1%>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>


                                                                                <asp:TemplateField HeaderText="Document">
                                                                                    <ItemTemplate>
                                                                                        <asp:HyperLink ID="HLDocumentLink" Target="_blank" runat="server" NavigateUrl='<%# Eval("DocumentLink") %>' Text='Download'>
                                                                                        </asp:HyperLink>
                                                                                        <asp:Label ID="lbl_DocumentLink" Visible="False" runat="server" Text='<%#Eval("DocumentLink")%>'></asp:Label>
                                                                                        <asp:HiddenField runat="server" ID="hfFileName" Value='<%#Eval("FileName")%>' />
                                                                                        <asp:HiddenField runat="server" ID="hfExtractedText" Value='<%#Eval("ExtractedText")%>' />

                                                                                        <asp:HiddenField runat="server" ID="hfDocumentLink" Value='<%#Eval("DocumentLink")%>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>


                                                                                <asp:TemplateField HeaderText="Summary Note	">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbl_DocumentNote" runat="server" Text='<%#Eval("DocumentNote") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>



                                                                                <asp:TemplateField HeaderText="Remove">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton runat="server" ID="btnDocRemove" OnClick="btnDocRemove_OnClick" CssClass="btn btn-sm btn-danger"><i class="fa fa-minus-circle"></i> </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>


                                                                    </div>
                                                                </div>
                                                            </fieldset>

                                                        </ContentTemplate>


                                                        <Triggers>

                                                            <asp:PostBackTrigger ControlID="btnDocUp" />

                                                        </Triggers>
                                                    </asp:UpdatePanel>

                                                    <div class="row" style="margin-top: 20px; margin-bottom: 10px;">
                                                        <div class="col-md-12 text-left">
                                                            <asp:LinkButton ID="submitButton" Visible="False" OnClick="btnSave_OnClick"
                                                                OnClientClick="return handleMeetingGridsSave() &amp;&amp; confirm('Are you sure you want to Submit ?')"
                                                                CssClass="btn btn-lg btn-info" runat="server">&nbsp; Submit</asp:LinkButton>
                                                            <asp:HiddenField runat="server" ID="id_mastetID" />
                                                            <asp:HiddenField runat="server" ID="hfGridA_Json" />
                                                            <asp:HiddenField runat="server" ID="hfGridB_Json" />
                                                            <asp:HiddenField runat="server" ID="hfGridA_ExistingCodes" />
                                                            <asp:LinkButton ID="editButton"
                                                                OnClientClick="return handleMeetingGridsSave() &amp;&amp; confirm('Are you sure you want to Update ?')"
                                                                CssClass="btn btn-lg btn-info" Visible="False" runat="server"
                                                                OnClick="editButton_OnClick">&nbsp; Update &amp; Submit</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div id="step-10">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="row">

                                                    <div class="col-md-3">
                                                        <h2 class="blue title-widget" style="color: #2196F3; text-shadow: 0 0 2px black;">Approval Path Setup</h2>
                                                    </div>
                                                    <hr />
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2" runat="server" visible="False">
                                                        <div class="form-group">

                                                            <asp:LinkButton runat="server" ID="LinkButtaon2" data-toggle="modal" data-target="#exampleModalAPP" CssClass="btn btn-sm btn-success">Choose Approval Path  </asp:LinkButton>
                                                        </div>

                                                    </div>

                                                    <div class="col-md-2">
                                                        <div class="form-group">

                                                            <asp:LinkButton runat="server" ID="LinkButton1" data-toggle="modal" data-target="#exampleModalBoardMember" CssClass="btn btn-sm btn-success">Choose Board-Member  </asp:LinkButton>
                                                        </div>

                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:UpdateProgress ID="UpdateProgress8" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                                    <ProgressTemplate>
                                                                        <div class="divWaiting">
                                                                            <asp:Image ID="imgWait4" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                                <div class="form-group">

                                                                    <asp:LinkButton runat="server" ID="btnAddMoreforApproval" data-toggle="modal" data-target="#exampleModalAPPEmpp" CssClass="btn btn-sm btn-secondary">Add More Member  in Approval Path  </asp:LinkButton>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                </div>




                                                <div class="row">


                                                    <div class="col-md-12">

                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:UpdateProgress ID="UpdateProgress9" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                                    <ProgressTemplate>
                                                                        <div class="divWaiting">
                                                                            <asp:Image ID="imgWait5" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                                <asp:GridView Width="100%" ShowHeader="True" ID="gv_ApprovalPathDetail" runat="server" AutoGenerateColumns="false" CssClass="blueTableNew" OnPreRender="gv_DocumentUpload_PreRender">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="SL#">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex + 1%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>


                                                                        <asp:TemplateField HeaderText="Mimimum Count for Approval">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkMimimumCount" AutoPostBack="True" OnCheckedChanged="chkMimimumCount_OnCheckedChanged" CssClass="form-control-sm" runat="server" />
                                                                                <asp:HiddenField runat="server" ID="hfIsMinimumApprovalPerson" Value='<%#Eval("IsMinimumApprovalPerson")%>' />


                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Sequence">
                                                                            <ItemTemplate>

                                                                                <asp:HiddenField runat="server" ID="hfSeq_No" Value='<%#Eval("Seq_No")%>' />

                                                                                <asp:DropDownList runat="server" ID="ddlSequenceList" class="form-control form-control-sm">
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Employee ID">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Appbl_EmpMasterCode" runat="server" Text='<%#Eval("EmpMasterCode") %>'></asp:Label>

                                                                                <asp:HiddenField runat="server" ID="ApphfEmpInfoId" Value='<%#Eval("EmpInfoId")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>


                                                                        <asp:TemplateField HeaderText="Employee Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Applbl_EmpName" runat="server" Text='<%#Eval("EmpName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Designation">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Applbl_Designation" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Is Edit">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkIsEdit" AutoPostBack="True" OnCheckedChanged="chkIsEdit_OnSelectedIndexChanged" CssClass="form-control-sm" runat="server" />
                                                                                <asp:HiddenField runat="server" ID="hfCanEdit" Value='<%#Eval("CanEdit")%>' />


                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>



                                                                        <asp:TemplateField HeaderText="Notification">
                                                                            <ItemTemplate>

                                                                                <asp:HiddenField runat="server" ID="HiNotificationEmailApp" Value='<%#Eval("NotificationEmail")%>' />
                                                                                <asp:HiddenField runat="server" ID="hfNotificationSMSApp" Value='<%#Eval("NotificationSMS")%>' />


                                                                                <asp:CheckBoxList runat="server" ID="chkNotificationApp" AutoPostBack="True" OnSelectedIndexChanged="chkNotificationApp_OnSelectedIndexChanged" CssClass="chkChoice" RepeatDirection="Horizontal">


                                                                                    <asp:ListItem>Email</asp:ListItem>
                                                                                    <asp:ListItem>SMS</asp:ListItem>
                                                                                </asp:CheckBoxList>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Remove">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton runat="server" ID="Appbtn_DetailsRemove" OnClick="Appbtn_DetailsRemove_OnClick" CssClass="btn btn-sm btn-danger"><i class="fa fa-minus-circle"></i> </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                    </Columns>
                                                                </asp:GridView>

                                                                <br />
                                                                <div class="row">

                                                                    <div class="col-md-6">
                                                                        <asp:Label runat="server" ID="lblstatus" Text="No Approval Path have been  selected." ForeColor="Red" Font-Size="17px"></asp:Label>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <br />

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>


                                                        <br />
                                                    </div>

                                                </div>
                                            </div>

                                            <br />
                                        </div>

                                    </div>
                                    <br />

                                </div>

                            </form>
                        </div>
                    </div>
                    <br />

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProgress10" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                <ProgressTemplate>
                                    <div class="divWaiting">
                                        <asp:Image ID="imgWait6" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <div class="row">



                                <div class="col-md-12">
                                    <div class="form-row">

                                        <div class="col-md-8">
                                            <link href="../UserSetup/ButtonGrups.css" rel="stylesheet" />
                                            <div class="ui-group-buttons">


                                                <%--<div class="or or-sm" runat="server"   id="orBTN"></div>--%>

                                                <asp:LinkButton ID="delButton" OnClientClick="return confirm('Are you sure you want to Delete ?')" Text="Delete" CssClass="btn btn-sm btn-danger" Visible="False" runat="server" OnClick="delButton_OnClick"> &nbsp; Delete</asp:LinkButton>

                                            </div>
                                        </div>

                                    </div>
                                    <div class="form-group">
                                    </div>
                                </div>


                                <div class="col-md-4">
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />

                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                </div>

                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>


            <!-- Trigger the modal with a button -->


            <!-- Modal -->

            <script type="text/javascript">
                $("body").on("click", "#btnUpload", function () {
                    debugger;
                    $.ajax({
                        url: '/HandlerDoc.ashx',
                        type: 'POST',
                        data: new FormData($('form')[0]),
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (file) {
                            $("#cpFormBody_hfDocFile").val('');
                            $("#cpFormBody_hfDocFileName").val('');
                            $("#fileProgress").hide();
                            $("#lblMessage").html("<b>" + file.name + "</b> has been uploaded.");
                             $("#cpFormBody_hfDocFile").val(file.dbfilename);
                             $("#cpFormBody_hfDocFileName").val(file.name);
                             if (window.updateMeetingWizardNextState) {
                                 window.updateMeetingWizardNextState();
                             }

                         },
                        xhr: function () {
                            var fileXhr = $.ajaxSettings.xhr();
                            if (fileXhr.upload) {
                                $("progress").show();
                                fileXhr.upload.addEventListener("progress", function (e) {
                                    if (e.lengthComputable) {
                                        $("#fileProgress").attr({
                                            value: e.loaded,
                                            max: e.total
                                        });
                                    }
                                }, false);
                            }
                            return fileXhr;
                        }
                    });
                });




             </script>

            <script type="text/javascript">
                (function () {
                    var ids = {
                        company: '<%= ddlCompany.ClientID %>',
                        title: '<%= txtTitle.ClientID %>',
                        category: '<%= ddlCategory.ClientID %>',
                        subCommitteeContainer: '<%= DivSubCommitte.ClientID %>',
                        subCommittee: '<%= ddlSubCommittee.ClientID %>',
                        meetingNote: '<%= txtMeetingpurpose.ClientID %>',
                        meetingDate: '<%= txtMeetingDate.ClientID %>',
                        documentGrid: '<%= gv_DocumentUpload.ClientID %>'
                    };

                    function byId(id) {
                        return document.getElementById(id);
                    }

                    function hasText(id) {
                        var control = byId(id);
                        return control && control.value.trim().length > 0;
                    }

                    function hasSelection(id) {
                        var control = byId(id);
                        return control && control.selectedIndex > 0 && control.value !== '' && control.value !== '-1';
                    }

                    function isVisible(id) {
                        var control = byId(id);
                        return control && control.offsetParent !== null;
                    }

                    function hasDocumentInList() {
                        var grid = byId(ids.documentGrid);
                        return !!grid && grid.querySelectorAll('tbody tr').length > 0;
                    }

                    function currentStepNumber() {
                        var wiz = $('.wizard').data('smartWizard');
                        if (wiz && typeof wiz.curStepIdx !== 'undefined') {
                            return wiz.curStepIdx + 1;
                        }
                        var selectedStep = document.querySelector('.wizard > ul.anchor > li > a.selected');
                        var stepNumber = selectedStep ? parseInt(selectedStep.getAttribute('rel'), 10) : 1;
                        return isNaN(stepNumber) ? 1 : stepNumber;
                    }

                    function isStepValid(stepNumber) {
                        if (stepNumber === 1) {
                            var subCommitteeValid = !isVisible(ids.subCommitteeContainer) || hasSelection(ids.subCommittee);
                            return hasSelection(ids.company)
                                && hasText(ids.title)
                                && hasSelection(ids.category)
                                && subCommitteeValid
                                && hasText(ids.meetingNote)
                                && hasText(ids.meetingDate);
                        }

                        if (stepNumber === 3) {
                            return hasDocumentInList();
                        }

                        return true;
                    }

                    function updateNextState() {
                        var nextButton = document.querySelector('.wizard .buttonNext');
                        if (!nextButton) {
                            return;
                        }

                        var enabled = isStepValid(currentStepNumber());
                        nextButton.classList.toggle('meeting-next-disabled', !enabled);
                        nextButton.classList.toggle('buttonDisabled', !enabled);
                        nextButton.classList.toggle('disabled', !enabled);
                        nextButton.setAttribute('aria-disabled', enabled ? 'false' : 'true');
                    }

                    function scrollWizardToTop() {
                        var wizard = document.querySelector('.wizard');
                        if (!wizard) {
                            return;
                        }

                        var top = wizard.getBoundingClientRect().top + window.pageYOffset - 85;
                        window.scrollTo(0, Math.max(0, top));
                    }

                    function refreshAfterStepNavigation() {
                        window.setTimeout(updateNextState, 50);
                        window.setTimeout(updateNextState, 200);
                        window.setTimeout(updateNextState, 500);
                        window.setTimeout(updateNextState, 800);
                        window.setTimeout(scrollWizardToTop, 100);
                    }

                    window.updateMeetingWizardNextState = updateNextState;

                    document.addEventListener('input', function (event) {
                        if (event.target.closest && event.target.closest('#step-7, #step-9')) {
                            updateNextState();
                        }
                    }, true);

                    document.addEventListener('change', function (event) {
                        if (event.target.closest && event.target.closest('#step-7, #step-9')) {
                            window.setTimeout(updateNextState, 0);
                        }
                    }, true);

                    document.addEventListener('click', function (event) {
                        var target = event.target.closest ? event.target.closest('.wizard .buttonNext, .wizard .buttonPrevious, .wizard > ul.anchor > li > a') : null;
                        if (!target) {
                            return;
                        }

                        var currentStep = currentStepNumber();
                        var isNextButton = target.classList.contains('buttonNext');
                        var isPrevButton = target.classList.contains('buttonPrevious');
                        var targetStep = isNextButton ? currentStep + 1 : (isPrevButton ? currentStep - 1 : parseInt(target.getAttribute('rel'), 10));

                        if (targetStep > currentStep && !isStepValid(currentStep)) {
                            event.preventDefault();
                            event.stopPropagation();
                            event.stopImmediatePropagation();
                            updateNextState();
                            return;
                        }

                        refreshAfterStepNavigation();
                    }, true);

                    document.addEventListener('keyup', function (event) {
                        var currentStep = currentStepNumber();
                        var isArrowRight = (event.key === 'ArrowRight' || event.keyCode === 39);
                        var isArrowLeft = (event.key === 'ArrowLeft' || event.keyCode === 37);

                        if (isArrowRight && !isStepValid(currentStep)) {
                            event.preventDefault();
                            event.stopPropagation();
                            event.stopImmediatePropagation();
                            updateNextState();
                            return;
                        }

                        if (isArrowRight || isArrowLeft) {
                            refreshAfterStepNavigation();
                        }
                    }, true);

                    function applyLocationVisibility() {
                        var rb = document.getElementById('<%= rbLocation.ClientID %>');
                        if (!rb) return;
                        var selectedVal = '';
                        var inputs = rb.getElementsByTagName('input');
                        for (var i = 0; i < inputs.length; i++) {
                            if (inputs[i].checked) {
                                selectedVal = inputs[i].value;
                                break;
                            }
                        }
                        var officePanel = document.querySelector('.location-panel-office');
                        var outerPanel = document.querySelector('.location-panel-outer');
                        var virtualPanel = document.querySelector('.location-panel-virtual');
                        if (officePanel) officePanel.style.display = (selectedVal === 'Office') ? '' : 'none';
                        if (outerPanel) outerPanel.style.display = (selectedVal === 'Outer') ? '' : 'none';
                        if (virtualPanel) virtualPanel.style.display = (selectedVal === 'Virtual') ? '' : 'none';
                    }

                    function initializeMeetingWizardValidation() {
                        window.setTimeout(updateNextState, 0);
                        applyLocationVisibility();

                        if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                            var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
                            pageRequestManager.add_endRequest(function () {
                                window.setTimeout(updateNextState, 0);
                                applyLocationVisibility();
                            });
                        }
                    }

                    if (document.readyState === 'loading') {
                        document.addEventListener('DOMContentLoaded', initializeMeetingWizardValidation);
                    } else {
                        initializeMeetingWizardValidation();
                    }
                })();
            </script>

            <style type="text/css">
                .emp-typeahead-results {
                    display: none;
                    position: absolute;
                    z-index: 1000;
                    left: 0;
                    right: 0;
                    top: 100%;
                    background: #fff;
                    border: 1px solid #ccc;
                    max-height: 220px;
                    overflow-y: auto;
                    box-shadow: 0 2px 6px rgba(0,0,0,0.15);
                }

                .emp-typeahead-option {
                    padding: 4px 8px;
                    cursor: pointer;
                    font-size: 13px;
                    white-space: nowrap;
                }

                .emp-typeahead-option:hover,
                .emp-typeahead-option.active {
                    background: #f0f0f0;
                }

                .emp-typeahead-empty {
                    padding: 4px 8px;
                    font-size: 12px;
                    color: #888;
                }
            </style>

            <script type="text/javascript">
                // Add Employees ("gv_Details_Save") — Employee Name search-as-you-type.
                // Looks employees up via AjaxSearchEmployeesForMeeting (scoped to the row's
                // selected Company, top 20 matches) instead of the old approach of binding
                // every active employee in the company into that row's DropDownList, which
                // put the whole roster into ViewState per Employee-type row on every postback.
                // Listeners are delegated on document so they keep working after this grid's
                // UpdatePanel re-renders a row (no re-init needed).
                (function () {
                    var searchTimer = null;
                    var MIN_TERM_LENGTH = 2;
                    var DEBOUNCE_MS = 250;

                    function closestRow(el) {
                        return el.closest ? el.closest('tr') : null;
                    }

                    function fieldInRow(row, idSuffix) {
                        return row.querySelector('[id$="_' + idSuffix + '"]');
                    }

                    function checkedRadioValueInRow(row, idSuffix) {
                        var container = fieldInRow(row, idSuffix);
                        if (!container) return '';
                        var checked = container.querySelector('input[type=radio]:checked');
                        return checked ? checked.value : '';
                    }

                    function hideResults(box) {
                        box.style.display = 'none';
                        box.innerHTML = '';
                    }

                    function selectEmployee(input, row, box, item) {
                        var rowIdx = row.getAttribute('data-row-idx');
                        if (window.MeetingGridA && MeetingGridA.isDuplicateEmployee(item.EmpMasterCode, parseInt(rowIdx, 10))) {
                            alert('Already Exist !!!');
                            hideResults(box);
                            return;
                        }

                        input.value = item.EmpName;
                        input.setAttribute('data-selected-name', item.EmpName);

                        var hfEmpInfo = fieldInRow(row, 'ShfEmpInfoId');
                        var hfCode = fieldInRow(row, 'txt_EmpMasterCode');
                        var txtDesg = fieldInRow(row, 'txt_Designation');

                        if (hfEmpInfo) hfEmpInfo.value = item.EmpInfoId;
                        if (hfCode) hfCode.value = item.EmpMasterCode;
                        if (txtDesg) txtDesg.value = item.Designation || '';

                        // The typeahead only mutates DOM fields directly (no input/change event),
                        // so pull the pick back into MeetingGridA's row array here.
                        if (window.MeetingGridA) MeetingGridA.syncRowFromDom(row);

                        hideResults(box);
                    }

                    function renderResults(input, row, box, items) {
                        box.innerHTML = '';
                        if (!items || !items.length) {
                            var empty = document.createElement('div');
                            empty.className = 'emp-typeahead-empty';
                            empty.textContent = 'No matching employee';
                            box.appendChild(empty);
                            box.style.display = 'block';
                            return;
                        }

                        items.forEach(function (item) {
                            var opt = document.createElement('div');
                            opt.className = 'emp-typeahead-option';
                            opt.textContent = item.EmpMasterCode + ' ; ' + item.EmpName +
                                (item.Designation ? ' (' + item.Designation + ')' : '');
                            // mousedown + preventDefault fires before the input's blur event,
                            // so the click registers before the results box gets hidden.
                            opt.addEventListener('mousedown', function (e) {
                                e.preventDefault();
                                selectEmployee(input, row, box, item);
                            });
                            box.appendChild(opt);
                        });
                        box.style.display = 'block';
                    }

                    function runSearch(input, row, box, companyId, term) {
                        $.ajax({
                            type: 'POST',
                            url: 'MeetingEntry.aspx/AjaxSearchEmployeesForMeeting',
                            data: JSON.stringify({ companyId: companyId, term: term }),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json'
                        }).done(function (response) {
                            var items = response && response.d ? response.d : [];
                            // Ignore stale responses for a box the user has already moved away from.
                            if (document.activeElement !== input) return;
                            renderResults(input, row, box, items);
                        }).fail(function () {
                            hideResults(box);
                        });
                    }

                    document.addEventListener('input', function (e) {
                        var input = e.target;
                        if (!input.classList || !input.classList.contains('emp-typeahead-input')) return;

                        // Editing the text after a suggestion was picked invalidates that pick.
                        if (input.getAttribute('data-selected-name') !== input.value) {
                            input.removeAttribute('data-selected-name');
                            var row0 = closestRow(input);
                            if (row0) {
                                var hfEmpInfo = fieldInRow(row0, 'ShfEmpInfoId');
                                var hfCode = fieldInRow(row0, 'txt_EmpMasterCode');
                                if (hfEmpInfo) hfEmpInfo.value = '';
                                if (hfCode) hfCode.value = '';
                            }
                        }

                        var row = closestRow(input);
                        var box = input.parentElement.querySelector('.emp-typeahead-results');
                        if (!row || !box) return;

                        var typeVal = checkedRadioValueInRow(row, 'rbType');
                        if (typeVal !== 'Employee') {
                            hideResults(box);
                            return;
                        }

                        var term = input.value.trim();
                        if (searchTimer) window.clearTimeout(searchTimer);

                        if (term.length < MIN_TERM_LENGTH) {
                            hideResults(box);
                            return;
                        }

                        var companyId = checkedRadioValueInRow(row, 'ddlCompanySave');
                        if (!companyId) {
                            hideResults(box);
                            return;
                        }

                        searchTimer = window.setTimeout(function () {
                            runSearch(input, row, box, companyId, term);
                        }, DEBOUNCE_MS);
                    }, true);

                    document.addEventListener('keydown', function (e) {
                        if (!e.target.classList || !e.target.classList.contains('emp-typeahead-input')) return;
                        if (e.key === 'Escape') {
                            var box = e.target.parentElement.querySelector('.emp-typeahead-results');
                            if (box) hideResults(box);
                        }
                    });

                    // Close an open results box when focus/clicks move elsewhere.
                    document.addEventListener('click', function (e) {
                        var wraps = document.querySelectorAll('.emp-typeahead-wrap');
                        wraps.forEach(function (wrap) {
                            if (!wrap.contains(e.target)) {
                                var box = wrap.querySelector('.emp-typeahead-results');
                                if (box) hideResults(box);
                            }
                        });
                    });
                })();
            </script>

            <script type="text/javascript">
                // Client-driven replacements for the old gv_Details_Save ("Add Employees") and
                // gv_BoardMember ("Members List") GridViews. Row state lives only in these two
                // in-memory arrays; the server never renders rows into the <tbody> markup again
                // after the first paint. render() is called from pageLoad() (see the script near
                // the top of this page), which MS AJAX invokes after the initial load AND after
                // every async postback — that's what makes these tables survive unrelated
                // UpdatePanel refreshes elsewhere on this page (they'd otherwise lose their rows
                // since the <tbody> markup itself gets wiped and re-sent empty on every such
                // postback). Only the page's Submit / Update & Submit buttons ever read this
                // state back into the server, via handleMeetingGridsSave() serializing both
                // arrays into hidden fields right before those buttons' postback fires.
                function escapeHtml(s) {
                    return String(s == null ? '' : s)
                        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
                        .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
                }

                window.MeetingGridA = (function () {
                    var rows = [];
                    var referenceData = { companies: [] };

                    function blankRow() {
                        return {
                            Type: '', CompanyId: '', EmpMasterCode: '', EmpInfoId: null,
                            EmpName: '', Designation: '', Position: '',
                            IsBoardMember: '0', BMemberSetupDetailsID: '0'
                        };
                    }

                    function normalize(r) {
                        return {
                            Type: r.Type || '',
                            CompanyId: (r.CompanyId === undefined || r.CompanyId === null) ? '' : String(r.CompanyId),
                            EmpMasterCode: r.EmpMasterCode || '',
                            EmpInfoId: (r.EmpInfoId === undefined || r.EmpInfoId === null || r.EmpInfoId === '') ? null : r.EmpInfoId,
                            EmpName: r.EmpName || '',
                            Designation: r.Designation || '',
                            Position: r.Position || '',
                            IsBoardMember: '0',
                            BMemberSetupDetailsID: (r.BMemberSetupDetailsID === undefined || r.BMemberSetupDetailsID === null || r.BMemberSetupDetailsID === '') ? '0' : String(r.BMemberSetupDetailsID)
                        };
                    }

                    function companyOptionsHtml(idx, selectedCompanyId, isDisabled) {
                        var html = '';
                        (referenceData.companies || []).forEach(function (c) {
                            // When Guest: never pre-check any company, and mark each radio disabled
                            var checked   = (!isDisabled && String(c.Value) === String(selectedCompanyId)) ? ' checked' : '';
                            var disabledAttr = isDisabled ? ' disabled' : '';
                            var labelStyle = isDisabled
                                ? 'margin-right:10px;font-weight:normal;opacity:0.45;cursor:not-allowed;'
                                : 'margin-right:10px;font-weight:normal;';
                            html += '<label style="' + labelStyle + '">' +
                                '<input type="radio" name="gridA_' + idx + '_company" value="' + escapeHtml(c.Value) + '"' + checked + disabledAttr + '> ' +
                                escapeHtml(c.TextField) + '</label>';
                        });
                        return html;
                    }

                    var POSITIONS = ['Member', 'Convenor', 'Secretary'];

                    function rowHtml(r, idx) {
                        var isGuest      = r.Type === 'Guest';
                        var typeEmployee = r.Type === 'Employee' ? ' checked' : '';
                        var typeGuest    = isGuest ? ' checked' : '';

                        var positionsHtml = POSITIONS.map(function (p) {
                            var checked = r.Position === p ? ' checked' : '';
                            return '<label style="margin-right:8px;font-weight:normal;"><input type="radio" name="gridA_' + idx + '_position" value="' + p + '"' + checked + '> ' + p + '</label>';
                        }).join('');

                        return '' +
                            '<tr data-row-idx="' + idx + '">' +
                            '<td>' + (idx + 1) + '</td>' +
                            '<td><span id="gridA_row_' + idx + '_rbType" class="chkChoice">' +
                            '<label style="margin-right:10px;font-weight:normal;"><input type="radio" name="gridA_' + idx + '_type" value="Employee"' + typeEmployee + '> Employee</label>' +
                            '<label style="font-weight:normal;"><input type="radio" name="gridA_' + idx + '_type" value="Guest"' + typeGuest + '> Guest</label>' +
                            '</span></td>' +
                            '<td><span id="gridA_row_' + idx + '_ddlCompanySave" class="chkChoice">' + companyOptionsHtml(idx, r.CompanyId, isGuest) + '</span></td>' +
                            '<td>' +
                            '<div class="emp-typeahead-wrap" style="position: relative; width: 220px;">' +
                            '<input type="text" id="gridA_row_' + idx + '_txt_EmpName" class="form-control form-control-sm emp-typeahead-input" autocomplete="off" style="width: 220px !important;" value="' + escapeHtml(r.EmpName) + '">' +
                            '<div class="emp-typeahead-results"></div>' +
                            '</div>' +
                            '<input type="hidden" id="gridA_row_' + idx + '_ShfEmpInfoId" value="' + escapeHtml(r.EmpInfoId == null ? '' : r.EmpInfoId) + '">' +
                            '<input type="text" id="gridA_row_' + idx + '_txt_EmpMasterCode" style="display:none" value="' + escapeHtml(r.EmpMasterCode) + '">' +
                            '</td>' +
                            '<td><input type="text" id="gridA_row_' + idx + '_txt_Designation" class="form-control form-control-sm" style="width: 160px !important;" value="' + escapeHtml(r.Designation) + '"></td>' +
                            '<td>' + positionsHtml + '</td>' +
                            '<td>' +
                            '<button type="button" class="btn btn-sm btn-success" data-action="addA" data-row-idx="' + idx + '"><i class="fa fa-plus"></i></button> ' +
                            '<button type="button" class="btn btn-sm btn-danger" data-action="removeA" data-row-idx="' + idx + '"><i class="fa fa-minus-circle"></i></button>' +
                            '</td>' +
                            '</tr>';
                    }

                    function render() {
                        var body = document.getElementById('gridA_Body');
                        if (!body) return;
                        body.innerHTML = rows.map(rowHtml).join('');
                    }

                    return {
                        get rows() { return rows; },
                        setReferenceData: function (companies) {
                            referenceData.companies = companies || [];
                        },
                        hydrate: function (jsonRows) {
                            rows = (jsonRows || []).map(normalize);
                            if (rows.length === 0) rows = [blankRow()];
                        },
                        appendRows: function (jsonRows) {
                            (jsonRows || []).forEach(function (r) { rows.push(normalize(r)); });
                        },
                        addRowAfter: function (idx) {
                            rows.splice(idx + 1, 0, blankRow());
                            render();
                        },
                        removeRow: function (idx) {
                            rows.splice(idx, 1);
                            if (rows.length === 0) rows = [blankRow()];
                            render();
                        },
                        isDuplicateEmployee: function (empMasterCode, excludeIdx) {
                            if (!empMasterCode) return false;
                            for (var i = 0; i < rows.length; i++) {
                                if (i === excludeIdx) continue;
                                if (rows[i].EmpMasterCode && rows[i].EmpMasterCode.toLowerCase() === String(empMasterCode).toLowerCase()) {
                                    return true;
                                }
                            }
                            return false;
                        },
                        syncRowFromDom: function (rowEl) {
                            var idx = parseInt(rowEl.getAttribute('data-row-idx'), 10);
                            if (isNaN(idx) || !rows[idx]) return;
                            var hfEmpInfo = rowEl.querySelector('[id$="_ShfEmpInfoId"]');
                            var hfCode = rowEl.querySelector('[id$="_txt_EmpMasterCode"]');
                            var txtName = rowEl.querySelector('[id$="_txt_EmpName"]');
                            var txtDesg = rowEl.querySelector('[id$="_txt_Designation"]');
                            rows[idx].EmpInfoId = hfEmpInfo && hfEmpInfo.value !== '' ? hfEmpInfo.value : null;
                            rows[idx].EmpMasterCode = hfCode ? hfCode.value : '';
                            rows[idx].EmpName = txtName ? txtName.value : '';
                            rows[idx].Designation = txtDesg ? txtDesg.value : '';
                        },
                        render: render
                    };
                })();

                window.MeetingGridB = (function () {
                    var rows = [];
                    var positionOptions = [];

                    function blankRow() {
                        return { EmpName: '', Designation: '', Position: '', BMemberSetupDetailsID: '0' };
                    }

                    function normalize(r) {
                        return {
                            EmpName: r.EmpName || '',
                            Designation: r.Designation || '',
                            Position: r.Position || '',
                            BMemberSetupDetailsID: (r.BMemberSetupDetailsID === undefined || r.BMemberSetupDetailsID === null || r.BMemberSetupDetailsID === '') ? '0' : String(r.BMemberSetupDetailsID)
                        };
                    }

                    function positionOptionsHtml(selectedText) {
                        return (positionOptions || []).map(function (p) {
                            var selected = p.TextField === selectedText ? ' selected' : '';
                            return '<option value="' + escapeHtml(p.Value) + '"' + selected + '>' + escapeHtml(p.TextField) + '</option>';
                        }).join('');
                    }

                    function rowHtml(r, idx) {
                        return '' +
                            '<tr data-row-idx="' + idx + '">' +
                            '<td>' + (idx + 1) + '</td>' +
                            '<td><input type="text" id="gridB_row_' + idx + '_txtBoardMember_EmpName" class="form-control form-control-sm" value="' + escapeHtml(r.EmpName) + '"></td>' +
                            '<td><input type="text" id="gridB_row_' + idx + '_txtBoardMember_Designation" class="form-control form-control-sm" value="' + escapeHtml(r.Designation) + '"></td>' +
                            '<td><select id="gridB_row_' + idx + '_ddlPosition" class="form-control form-control-sm SelectMe33">' + positionOptionsHtml(r.Position) + '</select></td>' +
                            '<td><button type="button" class="btn btn-sm btn-danger" data-action="removeB" data-row-idx="' + idx + '"><i class="fa fa-minus-circle"></i></button></td>' +
                            '</tr>';
                    }

                    function render() {
                        var body = document.getElementById('gridB_Body');
                        if (!body) return;
                        body.innerHTML = rows.map(rowHtml).join('');
                    }

                    // render() rebuilds every row purely from `rows[i].Position`, so any
                    // selection that hasn't (yet) made it into that array via the 'change'
                    // listener — e.g. a Chosen pick right before clicking Remove — would be
                    // silently dropped. Read the live DOM once, right before we destroy it,
                    // so nothing gets lost regardless of event timing.
                    function syncFromDom() {
                        var body = document.getElementById('gridB_Body');
                        if (!body) return;
                        var selects = body.querySelectorAll('select');
                        for (var i = 0; i < selects.length; i++) {
                            var sel = selects[i];
                            var tr = sel.closest ? sel.closest('tr') : null;
                            if (!tr) continue;
                            var idx = parseInt(tr.getAttribute('data-row-idx'), 10);
                            if (isNaN(idx) || !rows[idx]) continue;
                            rows[idx].Position = sel.options[sel.selectedIndex] ? sel.options[sel.selectedIndex].text : '';
                        }
                    }

                    return {
                        get rows() { return rows; },
                        setPositionOptions: function (positions) {
                            positionOptions = positions || [];
                        },
                        hydrate: function (jsonRows) {
                            rows = (jsonRows || []).map(normalize);
                            if (rows.length === 0) rows = [blankRow()];
                        },
                        removeRow: function (idx) {
                            syncFromDom();
                            rows.splice(idx, 1);
                            if (rows.length === 0) rows = [blankRow()];
                            render();
                        },
                        render: render
                    };
                })();

                // Structural changes (add/remove row) — these re-render, so Chosen (on Grid B's
                // Position <select>) gets destroyed/recreated only here, never on every keystroke.
                document.addEventListener('click', function (e) {
                    var btn = e.target.closest ? e.target.closest('[data-action]') : null;
                    if (!btn) return;
                    var idx = parseInt(btn.getAttribute('data-row-idx'), 10);
                    if (isNaN(idx)) return;

                    switch (btn.getAttribute('data-action')) {
                        case 'addA':
                            MeetingGridA.addRowAfter(idx);
                            break;
                        case 'removeA':
                            MeetingGridA.removeRow(idx);
                            break;
                        case 'removeB':
                            MeetingGridB.removeRow(idx);
                            break;
                    }

                    // These re-renders replace the row markup wholesale, which wipes any
                    // Chosen widget on Grid B's Position <select> — re-apply it so a
                    // previously-picked Position doesn't appear to reset after Add/Remove.
                    refreshChosenWidgets();
                });

                // Value-only changes (radios/select) — sync into the row array without a full
                // re-render, except Type/Company which also reset the row's employee fields
                // (matching the old rbType_OnSelectedIndexChanged / ddlCompanySave_SelectedIndexChanged
                // behavior) and so do need a re-render to reflect the clear + enable/disable.
                document.addEventListener('change', function (e) {
                    var el = e.target;
                    var rowEl = el.closest ? el.closest('tr') : null;
                    if (!rowEl) return;
                    var idx = parseInt(rowEl.getAttribute('data-row-idx'), 10);
                    if (isNaN(idx)) return;

                    if (rowEl.parentElement && rowEl.parentElement.id === 'gridA_Body') {
                        if (!MeetingGridA.rows[idx]) return;
                        if (el.type === 'radio' && el.name.indexOf('_type') > -1) {
                            var row = MeetingGridA.rows[idx];
                            row.Type = el.value;
                            row.EmpInfoId = null;
                            row.EmpMasterCode = '';
                            row.EmpName = '';
                            row.Designation = '';
                            if (el.value === 'Guest') row.CompanyId = '';
                            MeetingGridA.render();
                        } else if (el.type === 'radio' && el.name.indexOf('_company') > -1) {
                            var row2 = MeetingGridA.rows[idx];
                            row2.CompanyId = el.value;
                            row2.EmpInfoId = null;
                            row2.EmpMasterCode = '';
                            row2.EmpName = '';
                            row2.Designation = '';
                            MeetingGridA.render();
                        } else if (el.type === 'radio' && el.name.indexOf('_position') > -1) {
                            MeetingGridA.rows[idx].Position = el.value;
                        }
                    } else if (rowEl.parentElement && rowEl.parentElement.id === 'gridB_Body') {
                        if (!MeetingGridB.rows[idx]) return;
                        if (el.tagName === 'SELECT') {
                            MeetingGridB.rows[idx].Position = el.options[el.selectedIndex] ? el.options[el.selectedIndex].text : '';
                        }
                    }
                });

                // Plain keystrokes — sync only, never re-render (keeps focus/typing smooth and
                // leaves Grid B's Chosen widget untouched).
                document.addEventListener('input', function (e) {
                    var el = e.target;
                    var rowEl = el.closest ? el.closest('tr') : null;
                    if (!rowEl) return;
                    var idx = parseInt(rowEl.getAttribute('data-row-idx'), 10);
                    if (isNaN(idx)) return;

                    if (rowEl.parentElement && rowEl.parentElement.id === 'gridA_Body' && MeetingGridA.rows[idx]) {
                        if (el.id.indexOf('_txt_Designation') > -1) {
                            MeetingGridA.rows[idx].Designation = el.value;
                        } else if (el.classList.contains('emp-typeahead-input')) {
                            MeetingGridA.rows[idx].EmpName = el.value;
                        }
                    } else if (rowEl.parentElement && rowEl.parentElement.id === 'gridB_Body' && MeetingGridB.rows[idx]) {
                        if (el.id.indexOf('_txtBoardMember_EmpName') > -1) {
                            MeetingGridB.rows[idx].EmpName = el.value;
                        } else if (el.id.indexOf('_txtBoardMember_Designation') > -1) {
                            MeetingGridB.rows[idx].Designation = el.value;
                        }
                    }
                }, true);

                // Populates hfGridA_ExistingCodes right before a postback that needs to know
                // Grid A's current EmpMasterCodes server-side (the Search-Member modal's
                // "Add To List" / "Submit" buttons, which check for duplicates via CheckEmpList()).
                function prepareGridAExistingCodes() {
                    var codes = MeetingGridA.rows
                        .map(function (r) { return r.EmpMasterCode || ''; })
                        .filter(function (c) { return c; });
                    var hf = document.getElementById('<%= hfGridA_ExistingCodes.ClientID %>');
                    if (hf) hf.value = codes.join('|');
                }

                // Populates hfGridA_Json / hfGridB_Json right before the page's Submit / Update &
                // Submit postback, which is what SaveUpdateInfo() reads server-side instead of
                // walking GridView rows. Always returns true — the boolean is only there so it
                // can be chained with "&& confirm(...)" in OnClientClick and still let the
                // confirm's own answer decide whether the postback proceeds.
                function handleMeetingGridsSave() {
                    var hfA = document.getElementById('<%= hfGridA_Json.ClientID %>');
                    var hfB = document.getElementById('<%= hfGridB_Json.ClientID %>');
                    if (hfA) hfA.value = JSON.stringify(MeetingGridA.rows);
                    if (hfB) hfB.value = JSON.stringify(MeetingGridB.rows);
                    return true;
                }
            </script>

        </div>

    </div>
</asp:Content>

