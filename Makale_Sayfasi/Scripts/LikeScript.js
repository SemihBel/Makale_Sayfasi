$(function () {
    var divListe = [];

    $("div[data-makaleid]").each(function (i, e) {
        divListe.push($(e).data("makaleid"));
    });
        $.ajax({
            method: "POST",
            url: "/Makale/LikeMakale",
            data: { dizi: divListe }
        }).done(function (data) {
            if (data.sonuc != null && data.sonuc.length > 0)
            {
                for (var i = 0; i < data.sonuc.length; i++) {
                    var id = data.sonuc[i];
                    var likeMakale = $("div[data-makaleid=" + id + "]");
                    var btn = likeMakale.find("button[data-liked]");
                    btn.data("liked", true);

                    var span = btn.children().first();
                    span.removeClass("glyphicon-heart-empty");
                    span.addClass("glyphicon-heart");
                }
            }
        }).fail(function () {

        });

        $("button[data-liked]").click(function () {
            var btn = $(this)
            var liked = btn.data("liked");
            var makaleid = btn.data("makaleid");
            var spanKalp = btn.find("span.likekalp");
            var spanLikeSayisi = btn.find("span.likesayisi");

            $.ajax({
                method: "POST",
                url: "/Makale/LikeDurumuUpdate",
                data: { "makaleid": makaleid, "like": !liked }
            }).done(function (data) {

                if (data.hata)
                {
                    alert("Beğeni işlemi gerçekleşemedi.");
                }
                else {

                    liked = !liked;
                    btn.data("liked", liked);
                    spanLikeSayisi.text(data.sonuc);

                    spanKalp.removeClass("glyphicon-heart-empty");
                    spanKalp.removeClass("glyphicon-heart");


                    if (liked) {
                        spanKalp.addClass("glyphicon-heart");
                    }
                    else {
                        spanKalp.addClass("glyphicon-heart-empty");
                    }

                }

            }).fail(function () {


            });
        });
 

});