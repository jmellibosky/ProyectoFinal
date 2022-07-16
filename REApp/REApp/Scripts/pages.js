function Col(grid) {
    if (grid.getAttribute('data-Order')) {
        var data = grid.getAttribute('data-Order');
        data = data.substring(0, data.indexOf("-"));
        return parseInt(data);
    } else {
        return 0;
    }
}

function Asc(grid) {
    if (grid.getAttribute('data-Order')) {
        var data = grid.getAttribute('data-Order');
        return data.substring(data.indexOf("-") + 1);
    } else {
        return "asc";
    }
}

function Init() {
    if ($("table.data-table").length > 0) {
        // Recorre las tablas con la clase table.data-table.
        $("table.data-table").each(function () {
            // Agrega las clases Bootstrap
            $(this).addClass("table table-striped table-borderless no-footer dtr-column table-hover");

            // La tabla tiene filas?
            if ($(this).find("tbody tr").length > 1) {
                // Si la tabla tiene el plugin aplicado no se vuelve a aplicar.
                if (!$.fn.dataTable.isDataTable(this)) {
                    // Si la tabla no tiene thead se utiliza la primera fila del tbody como thead.
                    if ($(this).find("thead").length === 0) {
                        // Agrega el thead a la tabla.
                        $(this).prepend('<thead></thead>');

                        // Primera fila del tbody como thead (tbody.tr.td -> thead.tr.th)
                        $(this).find('thead').append($(this).find("tr:eq(0)"));
                    }

                    // Se agregan los data-order a las fechas y fechas horas para poder ordenarlas haciendo click en los titulos de las columnas.
                    $("#" + this.id + " td").each(function () {
                        // date-time
                        if (this.className === "date-time" || this.className === "date-only") {
                            var separador;

                            var data;

                            if ($(this).html().indexOf("/") !== -1) {
                                separador = "/";
                            }

                            if ($(this).html().indexOf("-") !== -1) {
                                separador = "-";
                            }

                            if ($(this).html().indexOf(".") !== -1) {
                                separador = ".";
                            }

                            if (typeof separador !== 'undefined') {
                                var fh = $(this).html().split(" ");

                                var sf = fh[0];

                                var d = sf.split(separador)[0];

                                var M = sf.split(separador)[1];

                                var y = sf.split(separador)[2];

                                if (this.className === "date-time") {
                                    var sh = fh[0];

                                    var h = sh.split(":")[0];

                                    var m = sh.split(":")[1];

                                    var s = sh.split(":")[2];

                                    if ($(this).html().indexOf("AM") > 0) {
                                        if (h === "12") h = "00";
                                    }

                                    if ($(this).html().indexOf("PM") > 0) {
                                        if (h !== 12) {
                                            h = parseInt(h) + 12;
                                        }
                                    }

                                    data = y + "-" + M + "-" + d + " " + h + ":" + m + ":" + s;
                                } else {
                                    data = y + "-" + M + "-" + d;
                                }

                                $(this).attr('data-Order', data);
                            }
                        }
                    });

                    // Si la tabla tiene al menos una columna que totalice se agrega el tfoot.
                    if ($(this).find('tr td.show-total').length > 0) {
                        // Si la tabla no tiene tfoot se clona desde el thead.
                        if ($(this).find("tfoot").length === 0) {
                            // Agrega el tfoot a la tabla.
                            $(this).append('<tfoot></tfoot>');

                            // Clona el thead en el tfoot.
                            $(this).find("thead tr").clone().appendTo($(this).find("tfoot"));

                            // Limpia el tfoot.
                            $(this).find("tfoot tr th").html('');
                        }
                    }

                    // Si luego de utilizar la primer fila de tbody com othead, la tabla no tiene filas en el tbody no se aplicac el plugin.
                    if ($(this).find("tbody tr").length > 0) {
                        // Parametros de inicializacion del plugin.
                        GridViewOptions = {
                            stateSave: false,
                            language: { // Idioma.
                                url: "../Vendors/DataTables/Spanish.json"
                            },
                            responsive: { // Responsive.
                                details: {
                                    type: 'column'
                                }
                            },
                            columnDefs: [
                                { // Primera columna con el +/- según las dimensiones de la pantalla.
                                    targets: 0,
                                    className: 'control'
                                },
                                { // Columna CryptoID excluida de las búsquedas utilizando el cuadro de texto buscar de DataTable
                                    targets: 1,
                                    searchable: false,
                                    className: 'd-none'
                                },
                                {
                                    responsivePriority: 1,
                                    targets: 2
                                },
                                {
                                    responsivePriority: 1,
                                    targets: -1
                                },
                                {
                                    type: 'currency',
                                    targets: "numeric-money"
                                }
                            ],
                            lengthMenu: ($(this).hasClass("no-paging")) ? [[-1], ["Todos"]] : [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todos"]],

                            // Si la grilla tiene la clase .no-paging se quita el paginado


                            // Evento para totalizar columnas.
                            "footerCallback": function (row, data, start, end, display) {
                                // Objeto tabla.
                                var api = this.api();

                                var procCols = "";

                                $(this).find('tr td.show-total').each(function () {
                                    // Indice de la columna.
                                    var colIndex = this.cellIndex;
                                    if ($(this).parent().first().html().trim().startsWith("<td class=\"crypto-id-column\">")) {
                                        colIndex++;
                                    }

                                    if (procCols.indexOf("|" + colIndex + "|") === -1) {
                                        procCols = procCols + "|" + colIndex + "|";

                                        var dataIndex = colIndex;

                                        // Totaliza la tabla.
                                        var table = api.column(dataIndex).data();
                                        var totalTable = 0;
                                        var tempTable = "";
                                        for (var i = 0; i < table.length; i++) {
                                            tempTable = table[i];

                                            // Se limpia el dato quitándole la configuración regional.
                                            tempTable = tempTable.replace("&nbsp;", 0);
                                            tempTable = tempTable.replace(regionalSettings.MoneySymbol, "");
                                            tempTable = tempTable.replace(regionalSettings.MoneyGroupSeparator, "");
                                            tempTable = tempTable.replace(regionalSettings.MoneyDecimalSymbol, ".");
                                            tempTable = tempTable.replace(" ", "");

                                            // Sumar o contar.
                                            if (isNaN(Number(tempTable))) {
                                                totalTable++;
                                            } else {
                                                totalTable = totalTable + Number(tempTable);
                                            }
                                        }
                                        totalTable = Math.round(totalTable * 100) / 100; // Redondea a 2 decimales.

                                        // Totaliza la página.
                                        var page = api.column(dataIndex, { page: 'current' }).data();
                                        var totalPage = 0;
                                        var tempPage = "";
                                        for (var n = 0; n < page.length; n++) {
                                            tempPage = page[n];

                                            // Se limpia el dato quitándole la configuración regional.
                                            tempPage = tempPage.replace("&nbsp;", 0);
                                            tempPage = tempPage.replace(regionalSettings.MoneySymbol, "");
                                            tempPage = tempPage.replace(regionalSettings.MoneyGroupSeparator, "");
                                            tempPage = tempPage.replace(regionalSettings.MoneyDecimalSymbol, ".");
                                            tempPage = tempPage.replace(" ", "");

                                            // Sumar o contar.
                                            if (isNaN(Number(tempPage))) {
                                                totalPage++;
                                            } else {
                                                totalPage = totalPage + Number(tempPage);
                                            }
                                        }
                                        totalPage = Math.round(totalPage * 100) / 100; // Redondea a 2 decimales.

                                        // Selector del tfoot.th
                                        if ($(this).parent().first().html().trim().startsWith("<td class=\"crypto-id-column\">")) {
                                            colIndex--;
                                        }
                                        var selector = 'tfoot th:eq(' + colIndex.toString() + ')';

                                        // Muestra los totales. Se aplica la misma clase que la columna para luego aplicar el plugin Autonumeric.
                                        var inputPage = '<span class="t_' + this.classList.value + '">' + totalPage + '</span>';
                                        var inputTable = '<span class="t_' + this.classList.value + '">' + totalTable + '</span>';

                                        // Se muestra el resultado.
                                        $(this).parent().parent().parent().find(selector).html(inputPage + "<br><b>" + inputTable + "</b>");

                                    }
                                });
                            }
                        };

                        // Aplica el plugin DataTable.
                        $(this).dataTable(GridViewOptions);
                        // Desactivo el stateSave para que no cachee la búsqueda realizada en el input "Buscar"
                        $(document).ready(function () {
                            $(this).DataTable({
                                stateSave: false
                            });
                        });

                        // Si tiene columnas busca el criterio de orden.
                        if (this.rows[0].cells.length > 1) {
                            // Tiene creiterio de orden?
                            if (this.getAttribute('data-Order')) {
                                // Ordena por el criterio.
                                $(this).DataTable().order([Col(this), Asc(this)]);
                            }
                        }

                        // Si la grilla tiene la clase .show-buttons se agregan los botones de exportación.
                        if ($(this).hasClass("show-buttons")) {

                            let columns = [];
                            for (var i = 3; i <= ($(this).children("tbody").children("tr:nth-child(1)").children("td").length); i++) {
                                var result = $(this).children("tbody")
                                    .children("tr:nth-child(1)")
                                    .children("td:nth-child(" + i + ")")
                                    .children().hasClass("exclude");
                                if ((result === false)) {
                                    columns.push(i - 1);
                                }
                            }

                            // Agrega los botones de exportacón inmediatamente despues del wrapper del datatable.
                            var buttons = new $.fn.dataTable.Buttons(this, {
                                buttons: [
                                    {
                                        extend: 'copy',
                                        exportOptions: {
                                            columns: columns,
                                            format: {
                                                body: function (data, row, column, node) {
                                                    return modifyData(data);
                                                }
                                            }
                                        }
                                    },
                                    {
                                        extend: 'excel',
                                        exportOptions: {
                                            columns: columns,
                                            format: {
                                                body: function (data, row, column, node) {
                                                    return modifyData(data);
                                                }
                                            }
                                        }
                                    },
                                    {
                                        extend: 'csv',
                                        exportOptions: {
                                            columns: columns,//':not(.no-exportar)'
                                            format: {
                                                body: function (data, row, column, node) {
                                                    return modifyData(data);
                                                }
                                            }
                                        }
                                    },
                                    {
                                        extend: 'print',
                                        exportOptions: {
                                            columns: columns,
                                            format: {
                                                body: function (data, row, column, node) {
                                                    return modifyData(data);
                                                }
                                            }
                                        }
                                    }
                                ]
                            }).container().appendTo($(this).parent());

                            $(".dt-buttons").addClass("text-center mt-3");

                            // copiar
                            $(".buttons-copy").html('<i class="fa fa-files-o" aria-hidden="true"></i>');
                            $(".buttons-copy").prop('title', 'Copiar');

                            // exportar a excel
                            $(".buttons-excel").html('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                            $(".buttons-excel").prop('title', 'Exportar a Excel');

                            // exportar a csv
                            $(".buttons-csv").html('<i class="fa fa-file-text-o" aria-hidden="true"></i>');
                            $(".buttons-csv").prop('title', 'Exportar a CSV');

                            // imprimir
                            $(".buttons-print").html('<i class="fa fa-print" aria-hidden="true"></i>');
                            $(".buttons-print").prop('title', 'Imprimir');

                            $("a.dt-button").addClass("btn-sm btn-primary");
                        } // fin validacion grilla con botones
                    } // fin validacion hay filas en el tbody despues de usar el primer tbody como thead
                } // fin de la validacion de tabla con el plugin ya aplicado
            } // fin de la validacion de filas en la grilla
        }); // fin del loop que recorre las table.data-table
    }


    //// .numeric-only --------------------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-only").length > 0) {
    //    var NumericIntegerOptions = {
    //        aForm: false,
    //        aPad: false,
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "keep",
    //        mDec: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        pSign: "p",
    //        vMax: "999999999",
    //        vMin: "0"
    //    };
    //    new AutoNumeric('.numeric-only', NumericIntegerOptions);
    //};

    //// .numeric-integer -----------------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-integer").length > 0) {
    //    var NumericIntegerOptions = {
    //        aForm: false,
    //        aPad: false,
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mDec: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        pSign: "p",
    //        vMax: "999999999",
    //        vMin: "-999999999"
    //    };
    //    new AutoNumeric('.numeric-integer', NumericIntegerOptions);
    //};

    //// .numeric-integer-positive --------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-integer-positive").length > 0) {
    //    var NumericIntegerPositiveOptions = {
    //        aForm: false,
    //        aPad: false,
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mDec: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        pSign: "p",
    //        vMax: "999999999",
    //        vMin: "0"
    //    };
    //    new AutoNumeric('.numeric-integer-positive', NumericIntegerPositiveOptions);
    //};

    //// .numeric-integer-negative --------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-integer-negative").length > 0) {
    //    var NumericIntegerNegativeOptions = {
    //        aForm: false,
    //        aPad: false,
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mDec: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        pSign: "p",
    //        vMax: "0",
    //        vMin: "-999999999"
    //    };
    //    new AutoNumeric('.numeric-integer-negative', NumericIntegerNegativeOptions);
    //};

    //// .numeric-decimal -----------------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-decimal").length > 0) {
    //    var NumericDecimalOptions = {
    //        aForm: false,
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        pSign: "p",
    //        vMax: "999999999.99",
    //        vMin: "-999999999.99"
    //    };
    //    new AutoNumeric('.numeric-decimal', NumericDecimalOptions);
    //};

    //// .numeric-decimal-positive --------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-decimal-positive").length > 0) {
    //    var NumericDecimalPositiveOptions = {
    //        aForm: false,
    //        aSign: "",
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        pSign: "p",
    //        vMax: "999999999.99",
    //        vMin: "0"
    //    };
    //    new AutoNumeric('.numeric-decimal-positive', NumericDecimalPositiveOptions);
    //};

    //// .numeric-decimal-negative --------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-decimal-negative").length > 0) {
    //    var NumericDecimalNegativeOptions = {
    //        aForm: false,
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        pSign: "p",
    //        vMax: "0",
    //        vMin: "-999999999.99"
    //    };
    //    new AutoNumeric('.numeric-decimal-negative', NumericDecimalNegativeOptions);
    //};

    //// .numeric-money -------------------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-money").length > 0) {
    //    var NumericMoneyOptions = {
    //        aForm: false,
    //        aSign: '$',
    //        dGroup: "3",
    //        lZero: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        vMax: "999999999.99",
    //        vMin: "-999999999.99"
    //    };
    //    new AutoNumeric('.numeric-money', NumericMoneyOptions);

    //    // Importes negativos en rojo.
    //    $('.numeric-money').change(function () {
    //        if (this.value.startsWith("-")) {
    //            $(this).css("color", "#d43b30");
    //            $(this).css("text-shadow", "0px 0px 1px rgba(235, 235, 235, 1)");
    //        }
    //    })
    //};

    //// .numeric-money-positive ----------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-money-positive").length > 0) {
    //    var NumericMoneyPositiveOptions = {
    //        aForm: false,
    //        aSign: '$',
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        vMax: "999999999.99",
    //        vMin: "0"
    //    };
    //    new AutoNumeric('.numeric-money-positive', NumericMoneyPositiveOptions);
    //};

    //// .numeric-money-negaive -----------------------------------------------------------------------------------------------------------------------
    //if ($(".numeric-money-negative").length > 0) {
    //    var NumericMoneyNegativeOptions = {
    //        aForm: false,
    //        aSign: '$',
    //        anDefault: null,
    //        dGroup: "3",
    //        lZero: "0",
    //        mRound: "S",
    //        nBracket: null,
    //        vMax: "0",
    //        vMin: "-999999999.99"
    //    };
    //    new AutoNumeric('.numeric-money-negative', NumericMoneyNegativeOptions);

    //    // Importes negativos en rojo.
    //    $(".numeric-money-negative").css("color", "#d43b30");
    //    $(".numeric-money-negative").css("text-shadow", "0px 0px 1px rgba(235, 235, 235, 1)");
    //};

    // .select-single -------------------------------------------------------------------------------------------------------------------------------
    //if ($(".select-single").length > 0) {
    //    $(".select-single").select2({
    //        theme: "bootstrap"
    //    });
    //}

    //// .select-multiple -----------------------------------------------------------------------------------------------------------------------------
    //if ($(".select-multiple").length > 0) {
    //    $(".select-multiple").attr("multiple", "multiple");
    //    $(".select-multiple").select2({
    //        theme: "bootstrap"
    //    });
    //}

    // .date-picker ---------------------------------------------------------------------------------------------------------------------------------
    if ($(".date-picker").length > 0) {

        // Idiomas.
        // Inglés - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        $.fn.datepicker.dates['en'] = {
            days: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            daysShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            daysMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
            months: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
            monthsShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            today: "Today",
            clear: "Clear",
            format: "mm/dd/yyyy",
            titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
            weekStart: 0
        };

        // Español - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        $.fn.datepicker.dates['es'] = {
            days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
            daysShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
            daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
            months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
            monthsShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
            today: "Hoy",
            clear: "Borrar",
            format: "dd/mm/yyyy",
            titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
            weekStart: 0
        };

        // parámetros de inicializacion del plugin DatePicker.
        var DatePickerOptions = {
            format: regionalSettings.DateFormat.replace("MM", "mm"),    // Formato de fecha.
            weekStart: regionalSettings.FirstDayWeek,                   // Primer día de la semana.

            // parametros fijos -------------------------------------------------------------------------
            todayBtn: true,                                             // Boton Hoy.
            autoclose: true,                                            // Cerrar al seleccionar.
            todayHighlight: true,                                       // Resaltar la fecha actual.
            language: 'es'                                              // Idioma.
        }
        $(".date-picker").datepicker(DatePickerOptions);
        $(".date-picker").prop('readonly', true);
    };

    // .dp-month-year ---------------------------------------------------------------------------------------------------------------------------------
    if ($(".dp-month-year").length > 0) {
        // Idiomas.
        // Inglés - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        $.fn.datepicker.dates['en'] = {
            days: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            daysShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            daysMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
            months: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
            monthsShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            today: "Today",
            clear: "Clear",
            autoclose: true,
            titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
            weekStart: 0
        };

        // Español - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        $.fn.datepicker.dates['es'] = {
            days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
            daysShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
            daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
            months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
            monthsShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
            today: "Hoy",
            clear: "Borrar",
            autoclose: true,
            titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
            weekStart: 0
        };

        var endDate = $(".dp-end-date-none").length > 0 ? '' : '0d';

        // parámetros de inicializacion del plugin DatePicker.
        var DatePickerMonthYearOptions = {
            format: regionalSettings.DateFormat.replace("MM", "mm"),    // Formato de fecha.
            weekStart: regionalSettings.FirstDayWeek,                   // Primer día de la semana.

            // parametros fijos -------------------------------------------------------------------------
            todayBtn: true,                                             // Boton Hoy.
            autoclose: true,                                            // Cerrar al seleccionar.
            todayHighlight: true,                                       // Resaltar la fecha actual.
            language: 'es',                                             // Idioma.
            minViewMode: 1,
            startView: "months",
            format: 'mm/yyyy',
            endDate: endDate
        }

        $(".dp-month-year").datepicker(DatePickerMonthYearOptions);
    };


    // .time-picker ---------------------------------------------------------------------------------------------------------------------------------
    if ($(".time-picker").length > 0) {
        var TimePickerOptions = {
            twelvehour: timeFormat,                                     // Formato de hora (12/24).

            // parametros fijos -------------------------------------------------------------------------
            autoclose: true                                             // El control se cierra al seleccionar los minutos.
        }
        $(".time-picker").timepicker(TimePickerOptions);
        $(".time-picker").prop('readonly', true);
    };

    // checkbox -------------------------------------------------------------------------------------------------------------------------------------
    if ($("input[type=checkbox]").length > 0) {
        $("input[type=checkbox]").each(function () {

            // Evita que el plugin se vuelva a aplicar sobre un checkbox que ya lo tiene aplicado.
            if (!$(this).data("switchery")) {
                var init = new Switchery(this, { size: 'small', color: '#5cb85c', secondaryColor: '#d9534f', jackColor: '#ffffffe6', jackSecondaryColor: '#ffffffe6' });
            }
        });
    };

    // .clickable -----------------------------------------------------------------------------------------------------------------------------------
    $('.clickable').on('click', 'td', function () {
        if (this.innerHTML != "") {
            if (typeof $(this).parent().find('td.crypto-id-column')[0] !== "undefined") {

                // Muestra el div procesando
                $("#busy").show();

                // CryptoID del Modelo
                var cryptoID = $(this).parent().find('td.crypto-id-column')[0].innerText;

                // Pagina destino
                var destPage = $(this).parent().parent().parent().attr("data-model");

                if (destPage.indexOf('?') >= 0) {
                    window.location.href = destPage + '&cryptoID=' + cryptoID;
                }
                else {
                    window.location.href = destPage + '?cryptoID=' + cryptoID;
                }
            }
        }
    });

    // .checkable -----------------------------------------------------------------------------------------------------------------------------------
    $('.checkable').on('click', 'td', function () {
        if (this.innerHTML.indexOf('type="checkbox"') == -1 && this.innerHTML != "") {

            // Checkbox.
            var checkbox = $(this).parent().find('td input:checkbox')[0];
            if (!checkbox.disabled) {
                if (checkbox.checked) {
                    checkbox.checked = false;
                } else {
                    checkbox.checked = true;
                }

                // Cambia el estado de switchery segun el estado del checkbox.
                if (typeof Event === 'function' || !document.fireEvent) {
                    var event = document.createEvent('HTMLEvents');
                    event.initEvent('change', true, true);
                    checkbox.dispatchEvent(event);
                } else {
                    checkbox.fireEvent('onchange');
                }
            }
        }
    });

    // .cuit ----------------------------------------------------------------------------------------------------------------------------------------
    if ($(".cuit").length > 0) {
        $(".cuit").mask("00-00000000-0");
    }

    // Importes negativos en las tablas en rojo.
    $('td.numeric-money, td.numeric-money-positive, td.numeric-money-negative').each(function () {
        if (this.innerText.startsWith("-")) {
            $(this).css("color", "#d43b30");
            $(this).css("text-shadow", "0px 0px 1px rgba(235, 235, 235, 1)");
        }
    });

    // Selecciona el contenido del control cuando se enfoca.
    $('.select-onfocus').focus(function () {
        $(this).select();
    })

    //$('.password-check').password({
    //    shortPass: 'La contraseña es demasiado corta',
    //    badPass: 'Débil. Combine mayúsculas, minúsculas, números',
    //    goodPass: 'Media. Incluya caracteres especiales',
    //    strongPass: '',
    //    containsUsername: 'La contraseña contiene el nombre de usuario',
    //    enterPass: 'Ingrese la contraseña',
    //    showPercent: false,
    //    showText: true, // shows the text tips
    //    animate: true, // whether or not to animate the progress bar on input blur/focus
    //    animateSpeed: 'fast', // the above animation speed
    //    username: false, // select the username field (selector or jQuery instance) for better password checks
    //    usernamePartialMatch: true, // whether to check for username partials
    //    minimumLength: 6 // minimum password length (below this threshold, the score is 0)
    //});
};