var makaleid = -1;

$('#myModal').on('show.bs.modal', function (e) {
    var btn = $(e.relatedTarget);
     makaleid = btn.data("makaleid");
    // do something...
    $('#myModal_body').load("/Yorum/YorumGoster/" + makaleid);
});

function yorumislem(btn, mode, yorumid, textid) {
    var editmod = $(btn).data("editmode");


    if (mode === "edit") {
        if (!editmod) {
            $(btn).removeClass("btn-warning");
            $(btn).addClass("btn-success");

            $(btn).find("span").removeClass("glyphicon-edit");
            $(btn).find("span").addClass("glyphicon-ok");

            $(btn).data("editmode", true);

            $(textid).addClass("editable");
            $(textid).attr("contenteditable", true);
        }
        else { //ters işlemlerini yapıyoruz

            $(btn).data("editmode", false);

            $(btn).addClass("btn-warning");
            $(btn).removeClass("btn-success");

            $(btn).find("span").addClass("glyphicon-edit");
            $(btn).find("span").removeClass("glyphicon-ok");

            $(textid).removeClass("editable");
            $(textid).attr("contenteditable", false);

            var yorum = $(textid).text();

            $.ajax({
                method: "POST",
                url: "/Yorum/Update/" + yorumid,
                data: { text: yorum }
            }).done(function (data) {
                if (data.sonuc) {
                    $('#myModal_body').load("/Yorum/YorumGoster/" + makaleid);
                }
                else {
                    alert("Yorum Güncellenemedi.");
                }

            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }




    }
    else if (mode == "delete") {
        var onay = confirm("Yorum silinsin mi?")
        if (!onay) return false;

        $.ajax({
            method: "GET",
            url: "/Yorum/YorumSil/" + yorumid

        }).done(function (data) {
            if (data.sonuc) {
                $('#myModal_body').load("/Yorum/YorumGoster/" + makaleid);
            }
            else {
                alert("Yorum Silinemedi.");
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.")
        });

    }
    else if (mode == "yorumekle") {
        var yorum = $("#yorum_text").val();
        $.ajax({
            method: "POST",
            url: "/Yorum/YorumEkle/",
            data: { "YorumMetni": yorum ,"makaleid": makaleid }

        }).done(function (data) {
            if (data.sonuc) {
                $('#myModal_body').load("/Yorum/YorumGoster/" + makaleid);
            }
            else {
                alert("Yorum Eklenemedi.");
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.")
        });

    }
}