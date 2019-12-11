$(document).ready(function () {

    var host = "http://" + window.location.host;
    var token = null;
    var headers = {};

    $("#odjava").css("display", "none");
    $("#registracija").css("display", "none");
    $("#prikaziRegistraciju").click(function () {
        $("#registracija").css("display", "block");
        $("#prijava").css("display", "none");
    });
    $("#registracija").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();
        var loz1 = $("#regLoz").val();
        var loz2 = $("#regLoz2").val();

        var sendData = {
            "Email": email,
            "Password": loz1,
            "ConfirmPassword": loz2
        };


        $.ajax({
            type: "POST",
            url: host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            $("#info").append("Uspešna registracija. Možete se prijaviti na sistem.");
            $("#regEmail").val('');
            $("#regLoz").val('');
            $("#regLoz2").val('');
            $("#priEmail").val('');
            $("#priLoz").val('');
            $("#registracija").css("display", "none");
            $("#prijava").css("display", "block");

        }).fail(function (data) {
            alert(data);
        });
    });


    $("#prijava").submit(function (e) {
        e.preventDefault();

        var email = $("#priEmail").val();
        var loz = $("#priLoz").val();

        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            "type": "POST",
            "url": host + "/Token",
            "data": sendData

        }).done(function (data) {
            console.log(data);
            $("#info").empty().append("Prijavljen korisnik: " + data.userName);
            token = data.access_token;
            $("#prijava").css("display", "none");
            $("#registracija").css("display", "none");
            $("#odjava").css("display", "block");
            $("#priEmail").val('');
            $("#priLoz").val('');


            forma.css("display", "block");
            formak.css("display", "block");

            $("#prikaziStatistika").css("display", "block");

            $("#data").css("display", "block");
            $("#dataKategorije").css("display", "block");
            ucitavanjeTabele();
            getNajmanji();

        }).fail(function (data) {
            alert(data);
        });
    });

    $("#odjavise").click(function () {
        token = null;
        headers = {};

        $("#prijava").css("display", "block");
        $("#registracija").css("display", "none");
        $("#odjava").css("display", "none");
        $("#info").empty();
        $("#sadrzaj").empty();

        forma.css("display", "none");
        formak.css("display", "none");
        $("#prikaziStatistika").css("display", "none");

        $("#data").css("display", "none");
        $("#dataKategorije").css("display", "none");


    });


    $("#prikaziStatistika").css("display", "none");

    $("#prikaziStatistika").click(function () {
        var url = host + "/api/statistika";
        $.ajax({
            type: "GET",
            url: url
        }).done(setStatistika).fail(function (data) {
            alert("doslo je do greske");
        });

    });

    function getNajmanji() {

        var url = host + "/api/najmanji";
        $.ajax({
            type: "GET",
            url: url
        }).done(setStatistika).fail(function (data) {
            alert("doslo je do greske");
        });
    }

    function setStatistika(data, status) {
        console.log("Status: " + status);

        var $container = $("#dataStatistika");
        $container.empty();

        if (status == "success") {
            console.log(data);

            var div = $("<div></div>");

            var h1 = $("<h1>Kategorije Statistika</h1>");

            div.append(h1);

            var table = $("<table class='table table-hover'></table>");


            var header = $("<thead><tr class=firstRow><td>Naziv</td><td>Ukupna Cena</td></tr></thead>");


            table.append(header);

            var tbody = $("<tbody></tbody>");
            for (i = 0; i < data.length; i++) {

                var row = $("<tr></tr>");


                var displayData = "<td>" + data[i].Naziv + "</td><td>" + data[i].UkupnaCena + "</td></tr></tbody>";
                row.append(displayData);

                tbody.append(row);
            }
            table.append(tbody);
            div.append(table);



            $container.append(div);
        } else {
            var div = $("<div></div>");
            var h1 = $("<h1>Greška prilikom preuzimanja podataka!</h1>");
            div.append(h1);
            $container.append(div);
        }
    };


    var urlKontrolera = "/api/proizvodi/";
    var urlKontroleraK = "/api/kategorije/"

    var forma = $("#dodavanje");
    var formak = $("#dodavanjeK");


    forma.css("display", "none");
    formak.css("display", "none");

    var formAction = "Create";
    var editingId;

    $("body").on("click", "#btnDelete", deleteData);
    $("body").on("click", "#btnEdit", editData);

    $("body").on("click", "#btnDeleteK", deleteDataK);
    $("body").on("click", "#btnEditK", editDataK);

    $("body").on("click", "#odustajanje", refreshTabele);
    $("body").on("click", "#odustajanjeK", refreshTabeleK);


    function ucitavanjeTabele() {
        if (token != null) {
            headers.Authorization = "Bearer " + token;
        }

        var requsetUrl = host + urlKontrolera;
        var requestUrlForDropdown = host + urlKontroleraK;

        console.log("Url:" + requsetUrl);
        console.log("Url:" + requestUrlForDropdown);

        $.ajax({
            type: "GET",
            url: requsetUrl,
            headers: headers
        }).done(setTabela);

        $.getJSON(requestUrlForDropdown, setDropdown);
        $.getJSON(requestUrlForDropdown, setKategorije);
    };

    function setDropdown(data, status) {

        var dropdown = $("#kategorijaDropdown");
        dropdown.empty();

        if (status == "success") {
            for (i = 0; i < data.length; i++) {
                var option = "<option value=" + data[i].Id + ">" + data[i].Naziv + "</option>";

                dropdown.append(option);
            }
        }
    };

    function setKategorije(data, status) {
        console.log("Status: " + status);

        var $container = $("#dataKategorije");
        $container.empty();

        if (status == "success") {
            console.log(data);
            var div = $("<div></div>");
            var h1 = $("<h1>Kategorije</h1>");
            div.append(h1);
            var table = $("<table class='table table-hover'></table>");
            var header = $("<thead><tr class=firstRow><td>Naziv</td></tr></thead>");
            table.append(header);
            var tbody = $("<tbody></tbody>");
            for (i = 0; i < data.length; i++) {
                var row = $("<tr></tr>");
                var displayData = "<td>" + data[i].Naziv + "</td></tr></tbody>";
                row.append(displayData);
                if (token != null) {
                    var stringId = data[i].Id.toString();
                    var displayDelete = "<td><button class='btn-danger btn' id=btnDeleteK name=" + stringId + ">Obrisi</button></td>";
                    var displayEdit = "<td><button class='btn btn-success' id=btnEditK name=" + stringId + ">Izmeni</button></td>";
                    row.append(displayDelete);
                    row.append(displayEdit);
                }
                tbody.append(row);
            }
            table.append(tbody);
            div.append(table);
            $container.append(div);
        } else {
            var div = $("<div></div>");
            var h1 = $("<h1>Greška prilikom preuzimanja podataka!</h1>");
            div.append(h1);
            $container.append(div);
        }
    };
    function setTabela(data, status) {
        console.log("Status: " + status);

        var $container = $("#data");
        $container.empty();
        if (token != null) {
            headers.Authorization = "Bearer " + token;
        }

        if (status == "success") {
            console.log(data);

            var div = $("<div></div>");

            var h1 = $("<h1>Proizvodi</h1>");

            div.append(h1);
            var table = $("<table class='table table-hover'></table>");

            var header = $("<thead><tr class=firstRow><td>Naziv</td><td>Kategorija</td><td>Cena</td></tr></thead>");

            table.append(header);

            var tbody = $("<tbody></tbody>");
            for (i = 0; i < data.length; i++) {
                var row = $("<tr></tr>");

                var displayData = "<td>" + data[i].Naziv + "</td><td>" + data[i].KategorijaProizvoda.Naziv + "</td><td>" + data[i].Cena + "</td></tr></tbody>";
                row.append(displayData);

                if (token != null) {
                    var stringId = data[i].Id.toString();
                    var displayDelete = "<td><button class='btn-danger btn' id=btnDelete name=" + stringId + ">Obrisi</button></td>";
                    var displayEdit = "<td><button class='btn btn-success' id=btnEdit name=" + stringId + ">Izmeni</button></td>";
                    row.append(displayDelete);
                    row.append(displayEdit);
                }

                tbody.append(row);
            }
            table.append(tbody);
            div.append(table);

            $container.append(div);
        } else {
            var div = $("<div></div>");
            var h1 = $("<h1>Greška prilikom preuzimanja podataka!</h1>");
            div.append(h1);
            $container.append(div);
        }
    };

    function deleteDataK() {
        var deleteId = this.name;

        if (token != null) {
            headers.Authorization = "Bearer " + token;
        }

        $.ajax({
            url: host + urlKontroleraK + deleteId.toString(),
            type: "DELETE",

            headers: headers
        })
            .done(function (data, status) {
                refreshTabele();
                refreshTabeleK();
            })
            .fail(function (data, status) {
                alert("Desila se greska!");
            });

    };

    function editDataK() {
        var editId = this.name;

        if (token != null) {
            headers.Authorization = "Bearer " + token;
        }

        $.ajax({
            url: host + urlKontroleraK + editId.toString(),
            type: "GET",
            headers: headers
        })
            .done(function (data, status) {

                $("#nazivKategorije").val(data.Naziv);

                editingId = data.Id;
                formAction = "Update";
            })
            .fail(function (data, status) {
                formAction = "Create";
                alert("Desila se greska!");
            });

    };
    $("#dodavanjeK").submit(function (e) {
        e.preventDefault();

        if (token != null) {
            headers.Authorization = 'Bearer ' + token;
        }
        var naziv = $("#nazivKategorije").val();
        var httpAction;
        var sendData;
        var url;

        if (formAction === "Create") {
            httpAction = "POST";
            url = host + urlKontroleraK;
            sendData = {
                "Naziv": naziv,
            };
        } else {
            httpAction = "PUT";
            url = host + urlKontroleraK + editingId.toString();
            sendData = {
                "Id": editingId,
                "Naziv": naziv
            };
        }


        console.log("Objekat za slanje");
        console.log(sendData);

        $.ajax({
            url: url,
            type: httpAction,
            headers: headers,
            data: sendData
        })
            .done(function (data, status) {
                formAction = "Create";
                refreshTabeleK();
            })
            .fail(function (data, status) {
                alert("Desila se greska!");
            })

    });


    function deleteData() {

        var deleteId = this.name;

        if (token != null) {
            headers.Authorization = "Bearer " + token;
        }

        $.ajax({
            url: host + urlKontrolera + deleteId.toString(),
            type: "DELETE",

            headers: headers
        })
            .done(function (data, status) {
                refreshTabele();
            })
            .fail(function (data, status) {
                alert("Desila se greska!");
            });

    };

    function editData() {
        var editId = this.name;

        if (token != null) {
            headers.Authorization = "Bearer " + token;
        }

        $.ajax({
            url: host + urlKontrolera + editId.toString(),
            type: "GET",
            headers: headers
        })
            .done(function (data, status) {

                $("#nazivProizvoda").val(data.Naziv);
                $("#cenaProizvoda").val(data.Cena);
                $("#kategorijaDropdown").val(data.KategorijaProizvodaId);

                editingId = data.Id;
                formAction = "Update";
            })
            .fail(function (data, status) {
                formAction = "Create";
                alert("Desila se greska!");
            });

    };

    $("#dodavanje").submit(function (e) {

        e.preventDefault();

        if (token != null) {
            headers.Authorization = 'Bearer ' + token;
        }

        var naziv = $("#nazivProizvoda").val();
        var cena = $("#cenaProizvoda").val();
        var kpid = $("#kategorijaDropdown").val();

        var httpAction;
        var sendData;
        var url;

        if (formAction === "Create") {
            httpAction = "POST";
            url = host + urlKontrolera;
            sendData = {
                "Naziv": naziv,
                "Cena": cena,
                "KategorijaProizvodaId": kpid
            };
        } else {
            httpAction = "PUT";
            url = host + urlKontrolera + editingId.toString();
            sendData = {
                "Id": editingId,
                "Naziv": naziv,
                "Cena": cena,
                "KategorijaProizvodaId": kpid
            };
        }


        console.log("Objekat za slanje");
        console.log(sendData);

        $.ajax({
            url: url,
            type: httpAction,
            headers: headers,
            data: sendData
        })
            .done(function (data, status) {
                formAction = "Create";
                refreshTabele();
            })
            .fail(function (data, status) {
                if (cena < 1 || cena > 1000) {
                    alert("Unesite cenu koja je veca od 0 a manja od 1000")
                }
                else {
                    alert("Desila se greska!");
                }

            })

    });


    function refreshTabele() {

        $("#nazivProizvoda").val('');
        $("#cenaProizvoda").val('');

        ucitavanjeTabele();
        getNajmanji();
    };

    function refreshTabeleK() {

        $("#nazivKategorije").val('');
        ucitavanjeTabele();
        getNajmanji();
    };

});