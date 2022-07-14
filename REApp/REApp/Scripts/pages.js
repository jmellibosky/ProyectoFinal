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

    if ($(".grid-view").length > 0) {

        $('.grid-view').each(function () {

            if ($(this).find("tbody tr").length > 1) {

                if (!$.fn.dataTable.isDataTable(this)) {

                    if ($(this).find("thead").length == 0) {

                        $(this).prepend('<thead></thead>');

                        $(this).find('thead').append($(this).find("tr:eq(0)"));
                    }

                    $("#" + this.id + " td").each(function () {

                        if (this.className == "date-time") {
                            var separador;
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
                                var d;
                                var M;
                                var y;

                                if (regionalSettings.DateFormat.replace("/", "").replace("-", "").replace(".", "").startsWith("ddMM")) {
                                    d = sf.split(separador)[0];
                                    M = sf.split(separador)[1];
                                    y = sf.split(separador)[2];
                                }
                                if (regionalSettings.DateFormat.replace("/", "").replace("-", "").replace(".", "").startsWith("MMdd")) {
                                    M = sf.split(separador)[0];
                                    d = sf.split(separador)[1];
                                    y = sf.split(separador)[2];
                                }
                                if (regionalSettings.DateFormat.replace("/", "").replace("-", "").replace(".", "").startsWith("yyyy")) {
                                    y = sf.split(separador)[0];
                                    M = sf.split(separador)[1];
                                    d = sf.split(separador)[2];
                                }

                                var sh = fh[1];
                                var h = sh.split(":")[0];
                                var m = sh.split(":")[1];
                                var s = sh.split(":")[2];

                                if ($(this).html().indexOf("AM") > 0) {
                                    if (h == "12") h = "00";
                                }

                                if ($(this).html().indexOf("PM") > 0) {
                                    if (h != 12) {
                                        h = parseInt(h) + 12;
                                    }
                                }

                                var data = y + "-" + M + "-" + d + " " + h + ":" + m + ":" + s;
                                $(this).attr('data-order', data);
                            }
                        }
                    });

                    if ($(this).find('tr td.show-total').length > 0) {

                        if ($(this).find("tfoot").length == 0) {

                            $(this).append('<tfoot></tfoot>');

                            $(this).find("thead tr").clone().appendTo($(this).find("tfoot"));

                            $(this).find("tfoot tr th").html('');
                        }
                    }

                    if ($(this).find("tbody tr").length > 0) {

                        GridViewOptions = {
                            language: {
                                url: "../Content/vendor/datatables.plugins/languages/" + regionalSettings.Language + ".json"
                            },
                            responsive: {
                                details: {
                                    type: 'column'
                                }
                            },
                            columnDefs: [
                                {
                                    targets: 0,
                                    className: 'control'
                                },
                                {
                                    targets: -1,
                                    className: 'all'
                                }],
                            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todos"]]
                        }

                        $(this).dataTable(GridViewOptions);

                        if (this.rows[0].cells.length > 1) {

                            if (this.getAttribute('data-Order')) {

                                $(this).DataTable().order([Col(this), Asc(this)]);
                            }
                        }

                        if ($(this).hasClass("show-buttons")) {

                            var buttons = new $.fn.dataTable.Buttons(this, {
                                buttons: [
                                    {
                                        extend: 'copy',
                                        exportOptions: {
                                            columns: ':visible'
                                        }
                                    },
                                    {
                                        extend: 'excel',
                                        exportOptions: {
                                            columns: ':visible'
                                        }
                                    },
                                    {
                                        extend: 'csv',
                                        exportOptions: {
                                            columns: ':visible'
                                        }
                                    },
                                    {
                                        extend: 'print',
                                        exportOptions: {
                                            columns: ':visible'
                                        }
                                    }
                                ]
                            }).container().appendTo($(this).parent());

                            $(".buttons-copy").html('<i class="fa fa-files-o" aria-hidden="true"></i>');
                            $('.buttons-copy').prop('title', 'Copiar');

                            $(".buttons-excel").html('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                            $('.buttons-excel').prop('title', 'Exportar a Excel');

                            $(".buttons-csv").html('<i class="fa fa-file-text-o" aria-hidden="true"></i>');
                            $('.buttons-csv').prop('title', 'Exportar a CSV');

                            $(".buttons-print").html('<i class="fa fa-print" aria-hidden="true"></i>');
                            $('.buttons-print').prop('title', 'Imprimir');

                            $("a.dt-button").addClass("btn-sm btn-primary");
                        }
                    }
                } 
            }

            $(this).fadeIn();
        });
    };

    // .numeric-only --------------------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-only").length > 0) {
        var NumericIntegerOptions = {
            aForm: false,
            aPad: false,
            anDefault: null,
            dGroup: "3",
            lZero: "keep",
            mDec: "0",
            mRound: "S",
            nBracket: null,
            pSign: "p",
            vMax: "999999999",
            vMin: "0"
        };
        $('.numeric-only').autoNumeric('init', NumericIntegerOptions);
    };

    // .numeric-integer -----------------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-integer").length > 0) {
        var NumericIntegerOptions = {
            aForm: false,
            aPad: false,
            anDefault: null,
            dGroup: "3",
            lZero: numericLeadZeros,
            mDec: "0",
            mRound: "S",
            nBracket: null,
            pSign: "p",
            vMax: "999999999",
            vMin: "-999999999"
        };
        $('.numeric-integer').autoNumeric('init', NumericIntegerOptions);
    };

    // .numeric-integer-positive --------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-integer-positive").length > 0) {
        var NumericIntegerPositiveOptions = {
            aForm: false,
            aPad: false,
            anDefault: null,
            dGroup: "3",
            lZero: numericLeadZeros,
            mDec: "0",
            mRound: "S",
            nBracket: null,
            pSign: "p",
            vMax: "999999999",
            vMin: "0"
        };
        $('.numeric-integer-positive').autoNumeric('init', NumericIntegerPositiveOptions);
    };

    // .numeric-integer-negative --------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-integer-negative").length > 0) {
        var NumericIntegerNegativeOptions = {
            aForm: false,
            aPad: false,
            anDefault: null,
            dGroup: "3",
            lZero: numericLeadZeros,
            mDec: "0",
            mRound: "S",
            nBracket: null,
            pSign: "p",
            vMax: "0",
            vMin: "-999999999"
        };
        $('.numeric-integer-negative').autoNumeric('init', NumericIntegerNegativeOptions);
    };

    // .numeric-decimal -----------------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-decimal").length > 0) {
        var NumericDecimalOptions = {
            aForm: false,
            anDefault: null,
            dGroup: "3",
            lZero: numericLeadZeros,
            mRound: "S",
            nBracket: null,
            pSign: "p",
            vMax: "999999999.99",
            vMin: "-999999999.99"
        };
        $('.numeric-decimal').autoNumeric('init', NumericDecimalOptions);
    };

    // .numeric-decimal-positive --------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-decimal-positive").length > 0) {
        var NumericDecimalPositiveOptions = {
            aForm: false,
            aSign: "",
            anDefault: null,
            dGroup: "3",
            lZero: numericLeadZeros,
            mRound: "S",
            nBracket: null,
            pSign: "p",
            vMax: "999999999.99",
            vMin: "0"
        };
        $('.numeric-decimal-positive').autoNumeric('init', NumericDecimalPositiveOptions);
    };

    // .numeric-decimal-negative --------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-decimal-negative").length > 0) {
        var NumericDecimalNegativeOptions = {
            aForm: false,
            anDefault: null,
            dGroup: "3",
            lZero: numericLeadZeros,
            mRound: "S",
            nBracket: null,
            pSign: "p",
            vMax: "0",
            vMin: "-999999999.99"
        };
        $('.numeric-decimal-negative').autoNumeric('init', NumericDecimalNegativeOptions);
    };

    // .numeric-money -------------------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-money").length > 0) {
        var NumericMoneyOptions = {
            aForm: false,
            aSign: '$',
            dGroup: "3",
            lZero: moneyLeadZeros,
            mRound: "S",
            nBracket: null,
            vMax: "999999999.99",
            vMin: "-999999999.99"
        };
        $('.numeric-money').autoNumeric('init', NumericMoneyOptions);

        // Importes negativos en rojo.
        $('.numeric-money').change(function () {
            if (this.value.startsWith("-")) {
                $(this).css("color", "#d43b30");
                $(this).css("text-shadow", "0px 0px 1px rgba(235, 235, 235, 1)");
            }
        })
    };

    // .numeric-money-positive ----------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-money-positive").length > 0) {
        var NumericMoneyPositiveOptions = {
            aForm: false,
            aSign: '$',
            anDefault: null,
            dGroup: "3",
            lZero: moneyLeadZeros,
            mRound: "S",
            nBracket: null,
            vMax: "999999999.99",
            vMin: "0"
        };
        $('.numeric-money-positive').autoNumeric('init', NumericMoneyPositiveOptions);
    };

    // .numeric-money-negaive -----------------------------------------------------------------------------------------------------------------------
    if ($(".numeric-money-negative").length > 0) {
        var NumericMoneyNegativeOptions = {
            aForm: false,
            aSign: '$',
            anDefault: null,
            dGroup: "3",
            lZero: moneyLeadZeros,
            mRound: "S",
            nBracket: null
            vMax: "0",
            vMin: "-999999999.99"
        };
        $('.numeric-money-negative').autoNumeric('init', NumericMoneyNegativeOptions);

        // Importes negativos en rojo.
        $(".numeric-money-negative").css("color", "#d43b30");
        $(".numeric-money-negative").css("text-shadow", "0px 0px 1px rgba(235, 235, 235, 1)");
    };

    // .phone (Argentina) -----------------------------------------------------------------------------------------------------------------------
    if ($(".phone").length > 0) {
        var PhoneOptions = {
            allowDecimalPadding: false,
            decimalPlaces: 0,
            digitGroupSeparator: "",
            leadingZero: "keep",
            maximumValue: "9999999999",
            minimumValue: "0"
        };
        $('.phone').autoNumeric('init', PhoneOptions);
    };

    // .select-single -------------------------------------------------------------------------------------------------------------------------------
    if ($(".select-single").length > 0) {
        $(".select-single").select2({
            theme: "bootstrap"
        });
    }

    // .select-multiple -----------------------------------------------------------------------------------------------------------------------------
    if ($(".select-multiple").length > 0) {
        $(".select-multiple").attr("multiple", "multiple");
        $(".select-multiple").select2({
            theme: "bootstrap"
        });
    }

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

    $('.password-check').password({
        shortPass: 'La contraseña es demasiado corta',
        badPass: 'Débil. Combine mayúsculas, minúsculas, números',
        goodPass: 'Media. Incluya caracteres especiales',
        strongPass: '',
        containsUsername: 'La contraseña contiene el nombre de usuario',
        enterPass: 'Ingrese la contraseña',
        showPercent: false,
        showText: true, // shows the text tips
        animate: true, // whether or not to animate the progress bar on input blur/focus
        animateSpeed: 'fast', // the above animation speed
        username: false, // select the username field (selector or jQuery instance) for better password checks
        usernamePartialMatch: true, // whether to check for username partials
        minimumLength: 6 // minimum password length (below this threshold, the score is 0)
    });
};