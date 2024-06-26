﻿$(document).ready(function () {
    // preloader
    //$(".preloader").show()
    //$(window).on("load", () => {
    //    $(".preloader").hide()
    //});
    setTimeout(function () { $(".preloader").hide(); }, 500);


    $.ajax({
        url: "/Basket/GetTotalSum",
        type: "GET",
        success: (data) => {
            if (data) { $("#sumBasket").text(data); } 
            else { console.log("Ошибка при получении значения сессии"); }
        },
        error: () => { console.log("Ошибка при отправки запроса для получении значения сессии"); }
    });

    // DataTable
    dataTable = $("#usersTable").DataTable({
        "ajax": {
            "url": "/User/GetUsers"
        },
        columns:
            [
                // для привязки данных используется модель UserVM
                { data: "shopUser.userName" },
                { data: "role" },
                { data: "shopUser.fullName" },
                { data: "shopUser.email" },
                { data: "shopUser.addressDelivery" },
                {
                    data: "shopUser.id",
                    render: function (data) {
                        return `<a href="/User/Edit/?id=${data}">Edit</a> |
                                <a href="/User/Delete/?id=${data}">Delete</a>`;
                    },
                    targets: -1,
                }
            ]
    });
    $("#user-btns div").hover(function () {
        $(this).css("cursor", "pointer");
    });
});
