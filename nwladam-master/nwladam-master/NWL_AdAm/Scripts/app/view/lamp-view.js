$(document).ready(function () {
    bindButtonByPermission();

    //-----------------------------------------------

    var modalState = MODAL_STATE.CREATE;

    //-----------------------------------------------

    var defaultPagingOptions = {
        onPageClick: function (e, page) {
            getLampData($("#controllerCode option:selected").val(), page, $("#lampTextSearch").val());
        }
    };

    //-----------------------------------------------

    function bindButtonByPermission() {
        if ($("#parentRole").attr("data-valkey") == "1") {
            $("#buttonNewLamp").addClass("invisible");
            $("#modal-save-button").addClass("d-none");
            $("#control-send-button").addClass("d-none");
        }
    }

    bindingChangeModeListDropDown();

    $("#schedule_1_start").datetimepicker({ format: "HH:mm", buttons: { showToday: true, showClose: true } });
    $("#schedule_2_start").datetimepicker({ format: "HH:mm", buttons: { showToday: true, showClose: true } });
    $("#schedule_3_start").datetimepicker({ format: "HH:mm", buttons: { showToday: true, showClose: true } });
    $("#schedule_4_start").datetimepicker({ format: "HH:mm", buttons: { showToday: true, showClose: true } });
    $("#schedule_5_start").datetimepicker({ format: "HH:mm", buttons: { showToday: true, showClose: true } });
    //$("#schedule_1_duration").datetimepicker({ format: "HH:mm" });
    //$("#schedule_2_duration").datetimepicker({ format: "HH:mm" });
    //$("#schedule_3_duration").datetimepicker({ format: "HH:mm" });
    //$("#schedule_4_duration").datetimepicker({ format: "HH:mm" });
    //$("#schedule_5_duration").datetimepicker({ format: "HH:mm" });

    if (document.location.search) {
        let queryString = {};

        $.each(document.location.search.substr(1).split('&'), function (_, params) {
            let i = params.split('=');
            queryString[i[0].toString()] = i[1].toString();
        });

        if (!$.isEmptyObject(queryString)) {
            fromDevicePageWithAsync(queryString);
        }
    } else {
        getProjectData("");
    }

    setInterval(function () {
        if ($("#isAutoRefresh").is(":checked")) {
            if (!($("#lampInfoModal").data('bs.modal') || {})._isShown && !($("#controlInfoModal").data('bs.modal') || {})._isShown) {
                if ($("#projectCode option:selected").attr("data-valkey") != "2") {
                    console.log("interval activate !");
                    getLampData($("#controllerCode option:selected").val(), $("#bottomPagination").twbsPagination("getCurrentPage"), $("#lampTextSearch").val());
                }
            }
        }
    }, 20000);

    async function fromDevicePageWithAsync(queryString) {
        await getProjectData(queryString["projectCode"]);

        $("#projectCode").val(queryString["projectCode"]).change(
            getControllerData(queryString["projectCode"], queryString["controllerCode"])
        );

        $("#controllerCode").val(queryString["projectCode"]).change(
            getLampData(queryString["controllerCode"], 1, "")
        );

        bindingAddButtonState();
    }

    //-----------------------------------------------

    function validateData() {
        let isValid = true;

        if (!$("#lampSerialNo").val().trim()) {
            $("#lampSerialNo").addClass("is-invalid")
            isValid = false;
        }

        if (!$("#lampName").val().trim()) {
            $("#lampName").addClass("is-invalid")
            isValid = false;
        }

        return isValid;
    };

    async function getProjectData(selectProjectCode) {
        $("#projectCode").LoadingOverlay("show");

        await fetch(ENDPOINT_URL.PROJECT_BY_CUSTOMER, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            }
        }).then(response => {
            $("#projectCode").LoadingOverlay("hide");
            return response.json();
        }).then(result => {
            let viewModel = JSON.parse(JSON.stringify(result));

            if (viewModel.state == "success") {
                bindingProjectDropdown(viewModel.data);

                if (viewModel.data.length > 0) {
                    if (selectProjectCode.trim().length > 0) {
                        $("#projectCode").val(selectProjectCode);
                    } else {
                        $("#projectCode").prop('selectedIndex', 0).change();
                    }
                }
            } else {
                showDialog(viewModel.state, viewModel.title, viewModel.message);
            }
        }).catch(error => {
            console.log(error.message);
            showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
        });
    };

    function bindingChangeModeListDropDown() {
        $("#controlChangeModeList").append("<option value='1'>กำหนดการทำงานด้วยตนเอง</option>");
        $("#controlChangeModeList").append("<option value='2'>ตั้งเวลาทำงานอัตโนมัติ</option>");

        if ($("#parentRole").attr("data-valkey") != "1") {
            $("#controlChangeModeList").append("<option value='-1'>DEBUG</option>");
            $("#controlChangeModeList").append("<option value='0'>CONFIG</option>");
            $("#controlChangeModeList").append("<option value='3'>AMBIENT_LIGHT</option>");
            $("#controlChangeModeList").append("<option value='4'>SCHEDULER_WITH_AMBIENT_LIGHT</option>");
        }
    };

    function bindingProjectDropdown(jsonProject) {
        if (jsonProject.length > 0) {
            $.each(jsonProject, function (_, item) {
                $("#projectCode").append($("<option value=" + item.projectCode + " data-valkey=" + item.projectType + ">" + item.projectName + "</option>"));
            });
        } else {
            $("#projectCode").append("<option selected disabled value=''>ไม่มีข้อมูล</option>");

            if (!$("#buttonNewLamp").hasClass("invisible")) {
                $("#buttonNewLamp").addClass("invisible");
            }
        }
    };

    function bindingAddButtonState() {
        if ($("#projectCode option:selected").attr("data-valkey") == "2") {
            if (!$("#buttonNewLamp").hasClass("invisible")) {
                $("#buttonNewLamp").addClass("invisible");
            }

            if ($("#EMMProjectWarning").hasClass("d-none")) {
                $("#EMMProjectWarning").removeClass("d-none")
            }

            if (!$("#no-more-tables").hasClass("d-none")) {
                $("#no-more-tables").addClass("d-none")
            }
        } else {
            if ($("#buttonNewLamp").hasClass("invisible")) {
                $("#buttonNewLamp").removeClass("invisible");
            }

            if (!$("#EMMProjectWarning").hasClass("d-none")) {
                $("#EMMProjectWarning").addClass("d-none")
            }

            if ($("#no-more-tables").hasClass("d-none")) {
                $("#no-more-tables").removeClass("d-none")
            }
        }

        bindButtonByPermission();
    }

    $("#projectCode").change(function (e) {
        bindingAddButtonState();

        $("#lampTextSearch").val("");
        $("#no-more-tables tbody").empty();

        getControllerData($("#projectCode option:selected").val(), "");
    });

    function getControllerData(projectCode, selectControllerCode) {
        $("#controllerCode").LoadingOverlay("show");

        let projectData = {
            projectCode: projectCode
        };

        fetch(ENDPOINT_URL.CONTROLLER_BY_PROJECT, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify(projectData)
        }).then(response => {
            $("#controllerCode").LoadingOverlay("hide");
            return response.json();
        }).then(result => {
            let viewModel = JSON.parse(JSON.stringify(result));

            if (viewModel.state == "success") {
                resetControllerDropDown();
                bindingControllerDropdown(viewModel.data);

                if (selectControllerCode.trim().length > 0) {
                    $("#controllerCode").val(selectControllerCode);
                } else {
                    $("#controllerCode").prop('selectedIndex', 0).change();
                }
            } else {
                showDialog(viewModel.state, viewModel.title, viewModel.message);
            }
        }).catch(error => {
            console.log(error.message);
            showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
        });
    };

    function bindingControllerDropdown(jsonController) {
        if (jsonController.length > 0) {
            $.each(jsonController, function (_, item) {
                $("#controllerCode").append($("<option value=" + item.controllerCode + ">" + item.controllerName + "</option>"));
            });
        } else {
            $("#controllerCode").append("<option selected disabled value=''>ไม่มีข้อมูล</option>");
        }
    };

    function resetControllerDropDown() {
        $("#controllerCode").empty();
    };

    $("#controllerCode").change(function (e) {
        getLampData($("#controllerCode option:selected").val(), 1, "");
    });

    function getLampData(controllerCode, currentPage, searchText) {
        if ($("#projectCode option:selected").attr("data-valkey") != "2") {
            $("#no-more-tables").LoadingOverlay("show");

            currentPage = typeof currentPage == "number" ? currentPage : 1;

            let lampData = {
                controllerCode: controllerCode,
                page: currentPage,
                searchText: searchText
            };

            fetch(ENDPOINT_URL.LAMP_LIST, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(lampData)
            }).then(response => {
                $("#no-more-tables").LoadingOverlay("hide");
                return response.json();
            }).then(result => {
                let viewModel = JSON.parse(JSON.stringify(result));

                if (viewModel.state == "success") {
                    if (currentPage > viewModel.pagingTotalPage) {
                        currentPage = viewModel.pagingTotalPage;
                    }

                    bindingLampTable(viewModel.data, viewModel.pagingTotalPage, currentPage);
                } else {
                    showDialog(viewModel.state, viewModel.title, viewModel.message);
                }
            }).catch(error => {
                console.log(error.message);
                showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
            });
        } else {
            if ($("#EMMProjectWarning").hasClass("d-none")) {
                $("#EMMProjectWarning").removeClass("d-none")
            }

            $("#no-more-tables tbody").empty();
            destroyPaging();

            return;
        }
    };

    function bindingLampTable(data, totalPage, currentPage) {
        if ($("#no-more-tables").hasClass("d-none")) {
            $("#no-more-tables").removeClass("d-none");
        }

        let tableBody = $("#no-more-tables tbody");
        tableBody.empty();

        let colSize = 4;
        if (data.length == 0) {
            tableBody.append("<tr><td colspan='" + colSize.toString() + "'>ไม่มีข้อมูล</td></tr>");
        } else {
            $.each(data, function (_, item) {
                let tableRow = jQuery("<tr></tr>");
                let serialNoCol = jQuery("<td></td>").attr("data-title", "S/N").html(item.lampCode).appendTo(tableRow);
                let lampNameCol = jQuery("<td></td>").attr("data-title", "ชื่อหลอดไฟ").html(item.lampName).appendTo(tableRow);

                let statusBadge;
                switch (item.lampStatus) {
                    case 0:
                        statusBadge = "badge-dark";
                        break;
                    case 1:
                        statusBadge = "badge-warning";
                        break;
                    case 2:
                        statusBadge = "badge-danger";
                        break;
                }
                let badgeHtml = "<span class=\"badge badge-pill " + statusBadge + "\">" + item.lampStatusText + "</span>";
                let lampStatusCol = jQuery("<td></td>").attr("data-title", "สถานะ").html(badgeHtml).appendTo(tableRow);

                let buttonCol = jQuery("<td></td>").appendTo(tableRow);

                let buttonArea = jQuery("<div></div>", {
                    class: "d-flex justify-content-end flex-wrap"
                }).appendTo(buttonCol);

                let controlButton = jQuery("<button></button>", {
                    type: "button",
                    class: "btn btn-light btn-icon"
                })
                    .attr("data-valkey", item.lampCode).html("<i class=\"mdi mdi-tune-vertical\"></i>")
                    .attr("data-status", item.lampStatus)
                    .on("click", showControlModal)
                    .appendTo(buttonArea);
                controlButton.tooltip({ title: "แผงควบคุม", boundary: "window", placement: "left" });

                let editButton = jQuery("<button></button>", {
                    type: "button",
                    class: "btn btn-light btn-icon ml-3"
                })
                    .attr("data-valkey", item.lampCode).html("<i class=\"mdi mdi-pencil\"></i>")
                    .on("click", showLampModalInEditMode)
                    .appendTo(buttonArea);
                editButton.tooltip({ title: "แก้ไขข้อมูล", boundary: "window", placement: "left" });

                if ($("#parentRole").attr("data-valkey") != "1") {
                    let deleteButton = jQuery("<button></button>", {
                        type: "button",
                        class: "btn btn-light btn-icon ml-3"
                    })
                        .attr("data-valkey", item.lampCode)
                        .on("click", promptDeleteLamp)
                        .html("<i class=\"mdi mdi-delete\"></i>")
                        .appendTo(buttonArea);
                    deleteButton.tooltip({ title: "ลบ", boundary: "window", placement: "left" });
                }

                tableBody.append(tableRow);
            });
        }

        destroyPaging();
        $("#bottomPagination").twbsPagination($.extend({}, defaultPagingOptions, {
            startPage: currentPage <= totalPage ? currentPage : totalPage,
            totalPages: totalPage
        }));
    };

    function destroyPaging() {
        if ($("#bottomPagination").data("twbs-pagination")) {
            $("#bottomPagination").twbsPagination("destroy");
        }
    };

    function showControlModal() {
        if ($(this).attr("data-status") == "2") {
            Swal.fire({
                title: "เกิดข้อผิดพลาด",
                text: "ไม่สามารถเชื่อมต่อหลอดไฟ \"" + $(this).attr("data-valkey") + "\"",
                icon: "error"
            });
            return;
        }

        $("#controlInfoModal").LoadingOverlay("show");
        fetch(ENDPOINT_URL.LAMP_STATUS, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify({ "lampCode": $(this).attr("data-valkey") })
        }).then(response => {
            $("#controlInfoModal").LoadingOverlay("hide");
            return response.json();
        }).then(result => {
            let viewModel = JSON.parse(JSON.stringify(result));

            if (viewModel.state == "success") {
                clearControlModalInput();
                bindControlPanelViewModelToModal(viewModel.data);
                $("#controlInfoModal").modal("show");
            } else {
                showDialog(viewModel.state, viewModel.title, viewModel.message);
            }
        }).catch(error => {
            console.log(error.message);
            showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
        });
    }

    function showLampModalInEditMode() {
        modalState = MODAL_STATE.UPDATE;

        $("#lampInfoModalLongTitle").text("ข้อมูลหลอดไฟ");
        $("#lampInfoModal").LoadingOverlay("show");

        fetch(ENDPOINT_URL.LAMP_INFO, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify({ "lampCode": $(this).attr("data-valkey") })
        }).then(response => {
            $("#lampInfoModal").LoadingOverlay("hide");
            return response.json();
        }).then(result => {
            let viewModel = JSON.parse(JSON.stringify(result));

            if (viewModel.state == "success") {
                bindLampViewModelToModal(viewModel.data);
                $("#lampInfoModal").modal("show");
            } else {
                showDialog(viewModel.state, viewModel.title, viewModel.message);
            }
        }).catch(error => {
            console.log(error.message);
            showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
        });
    };

    function promptDeleteLamp() {
        Swal.fire({
            title: "ยืนยันรายการ",
            text: "ลบหลอดไฟ \"" + $(this).attr("data-valkey") + "\" ออกจากระบบ?",
            icon: "question",
            confirmButtonColor: "#3085d6",
            confirmButtonText: "ลบ",
            showCancelButton: true,
            cancelButtonColor: "#d33",
            cancelButtonText: "ยกเลิก"
        }).then((result) => {
            if (result.isConfirmed) {
                $.LoadingOverlay("show");

                fetch(ENDPOINT_URL.LAMP_DELETE, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8"
                    },
                    body: JSON.stringify({ "lampCode": $(this).attr("data-valkey") })
                }).then(response => {
                    $.LoadingOverlay("hide");
                    return response.json();
                }).then(result => {
                    let viewModel = JSON.parse(JSON.stringify(result));

                    if (viewModel.state == "success") {
                        Swal.fire({
                            title: viewModel.title,
                            text: viewModel.message,
                            icon: viewModel.state,
                            confirmButtonColor: "#3085d6"
                        }).then((result) => {
                            if (viewModel.state == "success") {
                                getLampData($("#controllerCode option:selected").val(), $("#bottomPagination").twbsPagination("getCurrentPage"), $("#lampTextSearch").val());
                            }
                        });
                    } else {
                        showDialog(viewModel.state, viewModel.title, viewModel.message);
                    }
                }).catch(error => {
                    console.log(error.message);
                    showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
                });
            }
        });
    };

    function bindLampViewModelToModal(viewModel) {
        $("#lampSerialNo").val(viewModel.lampCode);
        $("#lampSerialNo").prop("disabled", true);

        $("#lampName").val(viewModel.lampName);
        $("#lampDescription").val(viewModel.lampDescription);
        $("#lampLat").val(viewModel.latitude);
        $("#lampLong").val(viewModel.longitude);
        $("#isWarning").prop("checked", viewModel.allowNotify);
    };

    $(".modal-state-create").on("click", function (e) {
        modalState = MODAL_STATE.CREATE;
        $("#lampInfoModalLongTitle").text("เพิ่มหลอดไฟใหม่");
    });

    $("#modal-save-button").on("click", function (e) {
        if (!validateData()) {
            return;
        }

        $("#lampInfoModal").LoadingOverlay("show");

        let postToUrl;
        switch (modalState) {
            case MODAL_STATE.CREATE:
                postToUrl = ENDPOINT_URL.LAMP_CREATE;
                break;
            case MODAL_STATE.UPDATE:
                postToUrl = ENDPOINT_URL.LAMP_UPDATE;
                break;
        };

        let controllerData = {
            lampCode: $("#lampSerialNo").val(),
            lampName: $("#lampName").val(),
            lampDescription: $("#lampDescription").val(),
            controllerCode: $("#controllerCode option:selected").val(),
            lampSerialNo: $("#lampSerialNo").val(),
            latitude: $("#lampLat").val(),
            longitude: $("#lampLong").val(),
            allowNotify: $("#isWarning").is(":checked")
        };

        fetch(postToUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify(controllerData)
        }).then(response => {
            $("#lampInfoModal").LoadingOverlay("hide");
            return response.json();
        }).then(result => {
            let viewModel = JSON.parse(JSON.stringify(result));

            if (viewModel.state == "success") {
                $("#lampInfoModal").modal("hide");
            }

            Swal.fire({
                title: viewModel.title,
                text: viewModel.message,
                icon: viewModel.state,
                confirmButtonColor: "#3085d6"
            }).then((result) => {
                if (viewModel.state == "success") {
                    getLampData($("#controllerCode option:selected").val(), $("#bottomPagination").twbsPagination("getCurrentPage"), $("#lampTextSearch").val());
                }
            });
        }).catch(error => {
            console.log(error.message);
            showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
        });
    });

    $("#lampInfoModal").on("hidden.bs.modal", function (e) {
        $("#lampSerialNo").val("");
        $("#lampSerialNo").prop("disabled", false);

        $("#lampName").val("");
        $("#lampDescription").val("");
        $("#lampLat").val("");
        $("#lampLong").val("");
        $("#isWarning").prop("checked", true);

        $("#lampSerialNo").removeClass("is-invalid")
        $("#lampName").removeClass("is-invalid")

    });

    $("#lampTextSearch").keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $("#lampSearchSubmit").click();
        }
    });

    $("#lampSearchSubmit").on("click", function (e) {
        getLampData($("#controllerCode option:selected").val(), 1, $("#lampTextSearch").val());
    });

    function bindControlPanelViewModelToModal(viewModel) {
        $("#controlMode").val(viewModel.mode);
        $("#controlProjectType").val(viewModel.projectType);
        $("#controlControllerCode").val(viewModel.controllerCode);
        $("#controlLampSerialNo").val(viewModel.lampSerialNo);
        $("#controlLampName").val(viewModel.lampName);
        $("#controlStaRelay").val(viewModel.relayName);
        $("#controlStaMode").val(viewModel.modeDescription);
        $("#controlStaCurrent").val(viewModel.current);
        $("#controlStaAmLight").val(viewModel.ambientLight);
        $("#controlStaPWM1_Text").text(viewModel.pwm1);
        $("#controlStaPWM1_rangeWarm").val(viewModel.pwm1);
        $("#controlStaPWM2_Text").text(viewModel.pwm2);
        $("#controlStaPWM2_rangeCool").val(viewModel.pwm2);

        if (viewModel.mode > 0) {
            $("#controlActionList").val(viewModel.modeName).change();
        }

        $("#controlRelayState").val(viewModel.relay).change();
        $("#rangeWarm").text(viewModel.pwm1);
        $("#controlRangeWarm").val(viewModel.pwm1);
        $("#rangeCool").text(viewModel.pwm2);
        $("#controlRangeCool").val(viewModel.pwm2);
        $("#controlUpdatedAt").text("ข้อมูลเมื่อ: " + moment(viewModel.updatedAt).format("yyyy/MM/DD HH:mm:ss"));
    };

    function hideControlActionList() {
        if (!$("#controlActionModeChange").hasClass("d-none")) {
            $("#controlActionModeChange").addClass("d-none");
        }

        if (!$("#controlActionManual").hasClass("d-none")) {
            $("#controlActionManual").addClass("d-none");
        }

        if (!$("#controlActionSchedule").hasClass("d-none")) {
            $("#controlActionSchedule").addClass("d-none");
        }
    };

    $("#controlActionList").change(function (e) {
        $("#controlInfoModal").LoadingOverlay("show");
        hideControlActionList();

        fetch(ENDPOINT_URL.MQTTCLIENT_ENDPOINT, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify({ "method": $(this).val() })
        }).then(response => { 
            $("#controlInfoModal").LoadingOverlay("hide");
            return response.json();
        }).then(result => {
            let viewModel = JSON.parse(JSON.stringify(result));

            if (viewModel.state == "success") {
                $("#controlEndpoint").attr("data-mqtt-url", viewModel.data);

                switch ($("#controlActionList").val()) {
                    case MQTT_PUBLISHER_ACTION.MODE_CHANGE:        
                        $("#controlActionModeChange").toggleClass("d-none");
                        $("#controlUpdatedAt").toggleClass("d-none");
                        break;
                    case MQTT_PUBLISHER_ACTION.MANUAL:
                        $("#controlActionManual").toggleClass("d-none");
                        $("#controlUpdatedAt").toggleClass("d-none");
                        break;
                    case MQTT_PUBLISHER_ACTION.SET_SCHEDULE:
                        $("#controlActionSchedule").toggleClass("d-none");
                        $("#controlUpdatedAt").toggleClass("d-none");
                        break;
                }
            } else {
                showDialog(viewModel.state, viewModel.title, viewModel.message);
            }
        }).catch(error => {
            console.log(error.message);
            showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
        });
    });

    function clearControlModalInput() {
        $("#rangeWarm").text("0");
        $("#controlRangeWarm").val("0");
        $("#rangeCool").text("0");
        $("#controlRangeCool").val("0");

        $("#schedule_1_start").val("00:00");
        $("#schedule_1_duration_h").val("0");
        $("#schedule_1_duration_m").val("0");
        $("#schedule_1_rangeWarm").text("0");
        $("#schedule_1_controlRangeWarm").val("0");
        $("#schedule_1_rangeCool").text("0");
        $("#schedule_1_controlRangeCool").val("0");

        $("#schedule_2_start").val("00:00");
        $("#schedule_2_duration_h").val("0");
        $("#schedule_2_duration_m").val("0");
        $("#schedule_2_rangeWarm").text("0");
        $("#schedule_2_controlRangeWarm").val("0");
        $("#schedule_2_rangeCool").text("0");
        $("#schedule_2_controlRangeCool").val("0");

        $("#schedule_3_start").val("00:00");
        $("#schedule_3_duration_h").val("0");
        $("#schedule_3_duration_m").val("0");
        $("#schedule_3_rangeWarm").text("0");
        $("#schedule_3_controlRangeWarm").val("0");
        $("#schedule_3_rangeCool").text("0");
        $("#schedule_3_controlRangeCool").val("0");

        $("#schedule_4_start").val("00:00");
        $("#schedule_4_duration_h").val("0");
        $("#schedule_4_duration_m").val("0");
        $("#schedule_4_rangeWarm").text("0");
        $("#schedule_4_controlRangeWarm").val("0");
        $("#schedule_4_rangeCool").text("0");
        $("#schedule_4_controlRangeCool").val("0");

        $("#schedule_5_start").val("00:00");
        $("#schedule_5_duration_h").val("0");
        $("#schedule_5_duration_m").val("0");
        $("#schedule_5_rangeWarm").text("0");
        $("#schedule_5_controlRangeWarm").val("0");
        $("#schedule_5_rangeCool").text("0");
        $("#schedule_5_controlRangeCool").val("0");
    };

    $("#controlInfoModal").on("hidden.bs.modal", function (e) {
        $("#controlEndpoint").attr("data-mqtt-url", "");

        switch ($("#controlActionList option:selected").val()) {
            case MQTT_PUBLISHER_ACTION.MODE_CHANGE:
                $("#controlActionModeChange").toggleClass("d-none");
                break;
            case MQTT_PUBLISHER_ACTION.MANUAL:
                $("#controlActionManual").toggleClass("d-none");
                break;
            case MQTT_PUBLISHER_ACTION.SET_SCHEDULE:
                $("#controlActionSchedule").toggleClass("d-none");
                break;
        }

        $("#controlUpdatedAt").toggleClass("d-none");
        $("#controlActionList").val("");
        clearControlModalInput();
    });

    $("#controlRelayState").change(function (e) {
        switch ($(this).val()) {
            case "0":
                $("#controlRangeWarm").prop("disabled", true);
                $("#controlRangeCool").prop("disabled", true);
                $("#controlRangeWarm").val("0");
                $("#controlRangeCool").val("0");
                $("#rangeWarm").text("0");
                $("#rangeCool").text("0");
                break;
            case "1":
                $("#controlRangeWarm").prop("disabled", false);
                $("#controlRangeCool").prop("disabled", false);
                break;
        }
    });

    $("#control-send-button").on("click", function (e) {
        $.LoadingOverlay("show");

        let endpoint = $("#controlEndpoint").attr("data-mqtt-url");
        let mqttReqData = {
            clientHost: MQTT_CONNECT.clientHost,
            clientPort: MQTT_CONNECT.clientPort,
            clientCredUser: MQTT_CONNECT.clientCredUser,
            clientCredPassword: MQTT_CONNECT.clientCredPassword,
            topicLevel: MQTT_CONNECT.topicLevel,
            topicProduct: MQTT_CONNECT.topicProduct,
            topicModel: MQTT_CONNECT.topicModel,
            topicGroup: MQTT_CONNECT.topicGroup,
            topicSerialNo: $("#controlLampSerialNo").val(),
            _currentMode: $("#controlMode").val()
        }

        switch ($("#controlActionList option:selected").val()) {
            case MQTT_PUBLISHER_ACTION.MODE_CHANGE:
                mqttReqData.payload = {
                    tsID: moment().format("YYMMDD-HHmmss-SSS"),
                    CMD: MQTT_CMD.CHANGE_MODE,
                    MODE: $("#controlChangeModeList option:selected").val()
                };

                if ($("#controlProjectType").val() == "1") {
                    mqttReqData.payload.SSID = $("#controlControllerCode").val();
                }
                break;

            case MQTT_PUBLISHER_ACTION.MANUAL:
                mqttReqData.payload = {
                    tsID: moment().format("YYMMDD-HHmmss-SSS"),
                    CMD: MQTT_CMD.MANUAL,
                    Relay: $("#controlRelayState option:selected").val(),
                    PWM1: $("#controlRangeWarm").val(),
                    PWM2: $("#controlRangeCool").val()
                };

                if ($("#controlProjectType").val() == "1") {
                    mqttReqData.payload.SSID = $("#controlControllerCode").val();
                }
                break;

            case MQTT_PUBLISHER_ACTION.SET_SCHEDULE:
                mqttReqData.payload = [
                    {
                        tsID: moment().format("YYMMDD-HHmmss-SSS"),
                        CMD: MQTT_CMD.SET_SCHEDULE,
                        ScheduleNo: $("#schedule_1_no").val(),
                        Start: $("#schedule_1_start").val(),
                        //Duration: $("#schedule_1_duration").val(),
                        Duration: $("#schedule_1_duration_h").val() + ":" + $("#schedule_1_duration_m").val(),
                        PWM1: $("#schedule_1_controlRangeWarm").val(),
                        PWM2: $("#schedule_1_controlRangeCool").val()
                    }
                    , {
                        tsID: moment().format("YYMMDD-HHmmss-SSS"),
                        CMD: MQTT_CMD.SET_SCHEDULE,
                        ScheduleNo: $("#schedule_2_no").val(),
                        Start: $("#schedule_2_start").val(),
                        //Duration: $("#schedule_2_duration").val(),
                        Duration: $("#schedule_2_duration_h").val() + ":" + $("#schedule_2_duration_m").val(),
                        PWM1: $("#schedule_2_controlRangeWarm").val(),
                        PWM2: $("#schedule_2_controlRangeCool").val()
                    }
                    , {
                        tsID: moment().format("YYMMDD-HHmmss-SSS"),
                        CMD: MQTT_CMD.SET_SCHEDULE,
                        ScheduleNo: $("#schedule_3_no").val(),
                        Start: $("#schedule_3_start").val(),
                        //Duration: $("#schedule_3_duration").val(),
                        Duration: $("#schedule_3_duration_h").val() + ":" + $("#schedule_3_duration_m").val(),
                        PWM1: $("#schedule_3_controlRangeWarm").val(),
                        PWM2: $("#schedule_3_controlRangeCool").val()
                    }
                    , {
                        tsID: moment().format("YYMMDD-HHmmss-SSS"),
                        CMD: MQTT_CMD.SET_SCHEDULE,
                        ScheduleNo: $("#schedule_4_no").val(),
                        Start: $("#schedule_4_start").val(),
                        //Duration: $("#schedule_4_duration").val(),
                        Duration: $("#schedule_4_duration_h").val() + ":" + $("#schedule_4_duration_m").val(),
                        PWM1: $("#schedule_4_controlRangeWarm").val(),
                        PWM2: $("#schedule_4_controlRangeCool").val()
                    }
                    , {
                        tsID: moment().format("YYMMDD-HHmmss-SSS"),
                        CMD: MQTT_CMD.SET_SCHEDULE,
                        ScheduleNo: $("#schedule_5_no").val(),
                        Start: $("#schedule_5_start").val(),
                        //Duration: $("#schedule_5_duration").val(),
                        Duration: $("#schedule_5_duration_h").val() + ":" + $("#schedule_5_duration_m").val(),
                        PWM1: $("#schedule_5_controlRangeWarm").val(),
                        PWM2: $("#schedule_5_controlRangeCool").val()
                    }
                ];

                if ($("#controlProjectType").val() == "1") {
                    mqttReqData.payload[0].SSID = $("#controlControllerCode").val();
                    mqttReqData.payload[1].SSID = $("#controlControllerCode").val();
                    mqttReqData.payload[2].SSID = $("#controlControllerCode").val();
                    mqttReqData.payload[3].SSID = $("#controlControllerCode").val();
                    mqttReqData.payload[4].SSID = $("#controlControllerCode").val();
                }
                break;

            default:
                mqttReqData = "";
                break;
        }

        if (!$.isEmptyObject(mqttReqData)) {
            fetch(endpoint, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(mqttReqData)
            }).then(response => {
                $.LoadingOverlay("hide");
                return response.json();
            }).then(result => {
                let viewModel = JSON.parse(JSON.stringify(result));

                if (viewModel.status == "OK") {
                    Swal.fire({
                        title: "ดำเนินการสำเร็จ",
                        text: "ส่งคำสั่งไปหลอดไฟ \"" + $("#controlLampSerialNo").val() + "\" แล้ว",
                        icon: "success",
                        confirmButtonColor: "#3085d6"
                    });
                    $("#controlInfoModal").modal("hide");
                } else {
                    Swal.fire({
                        title: "เกิดข้อผิดพลาด",
                        text: "ส่งคำสั่งไม่สำเร็จ",
                        icon: "error",
                        confirmButtonColor: "#3085d6"
                    });
                }
            }).catch(error => {
                console.log(error.message);
                showDialog("error", "เกิดข้อผิดพลาด", "กรุณาติดต่อผู้ดูแลระบบ");
            });
        }
    });

    $('.numberInputValidate').keypress(function (e) {
        return numberInputValidate(e);
    });

    $('.numberInputValidate').blur(function (e) {
        if (e.currentTarget.value == "") {
            e.currentTarget.value = "0";
        }
    });
});