/* Set the defaults for DataTables initialisation */
$.extend(true, $.fn.dataTable.defaults, {
    "sDom": "<'row'<'col-sm-6'l><'col-sm-6'f>r>" + "t" + "<'row'<'col-sm-6'i><'col-sm-6'p>>",
    "oLanguage": {
        "sLengthMenu": "_MENU_ records per page"
    }
});


/* Default class modification */
$.extend($.fn.dataTableExt.oStdClasses, {
    "sWrapper": "dataTables_wrapper form-inline",
    "sFilterInput": "form-control input-sm",
    "sLengthSelect": "form-control input-sm"
});

// In 1.10 we use the pagination renderers to draw the Bootstrap paging,
// rather than  custom plug-in
if ($.fn.dataTable.Api) {
    $.fn.dataTable.defaults.renderer = 'bootstrap';
    $.fn.dataTable.ext.renderer.pageButton.bootstrap = function(settings, host, idx, buttons, page, pages) {
        var api = new $.fn.dataTable.Api(settings);
        var classes = settings.oClasses;
        var lang = settings.oLanguage.oPaginate;
        var btnDisplay, btnClass;

        var attach = function(container, buttons) {
            var i, ien, node, button;
            var clickHandler = function(e) {
                e.preventDefault();
                if (e.data.action !== 'ellipsis') {
                    api.page(e.data.action).draw(false);
                }
            };

            for (i = 0, ien = buttons.length; i < ien; i++) {
                button = buttons[i];

                if ($.isArray(button)) {
                    attach(container, button);
                } else {
                    btnDisplay = '';
                    btnClass = '';

                    switch (button) {
                        case 'ellipsis':
                            btnDisplay = '&hellip;';
                            btnClass = 'disabled';
                            break;

                        case 'first':
                            btnDisplay = lang.sFirst;
                            btnClass = button + (page > 0 ?
                                '' : ' disabled');
                            break;

                        case 'previous':
                            btnDisplay = lang.sPrevious;
                            btnClass = button + (page > 0 ?
                                '' : ' disabled');
                            break;

                        case 'next':
                            btnDisplay = lang.sNext;
                            btnClass = button + (page < pages - 1 ?
                                '' : ' disabled');
                            break;

                        case 'last':
                            btnDisplay = lang.sLast;
                            btnClass = button + (page < pages - 1 ?
                                '' : ' disabled');
                            break;

                        default:
                            btnDisplay = button + 1;
                            btnClass = page === button ?
                                'active' : '';
                            break;
                    }

                    if (btnDisplay) {
                        node = $('<li>', {
                            'class': classes.sPageButton + ' ' + btnClass,
                            'aria-controls': settings.sTableId,
                            'tabindex': settings.iTabIndex,
                            'id': idx === 0 && typeof button === 'string' ? settings.sTableId + '_' + button : null
                        })
                            .append($('<a>', {
                                    'href': '#'
                                })
                                .html(btnDisplay)
                        )
                            .appendTo(container);

                        settings.oApi._fnBindAction(
                            node, {
                                action: button
                            }, clickHandler
                        );
                    }
                }
            }
        };

        attach(
            $(host).empty().html('<ul class="pagination"/>').children('ul'),
            buttons
        );
    }
} else {
    // Integration for 1.9-
    $.fn.dataTable.defaults.sPaginationType = 'bootstrap';

    /* API method to get paging information */
    $.fn.dataTableExt.oApi.fnPagingInfo = function(oSettings) {
        return {
            "iStart": oSettings._iDisplayStart,
            "iEnd": oSettings.fnDisplayEnd(),
            "iLength": oSettings._iDisplayLength,
            "iTotal": oSettings.fnRecordsTotal(),
            "iFilteredTotal": oSettings.fnRecordsDisplay(),
            "iPage": oSettings._iDisplayLength === -1 ? 0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
            "iTotalPages": oSettings._iDisplayLength === -1 ? 0 : Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
        };
    };

    /* Bootstrap style pagination control */
    $.extend($.fn.dataTableExt.oPagination, {
        "bootstrap": {
            "fnInit": function(oSettings, nPaging, fnDraw) {
                var oLang = oSettings.oLanguage.oPaginate;
                var fnClickHandler = function(e) {
                    e.preventDefault();
                    if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                        fnDraw(oSettings);
                    }
                };

                $(nPaging).append(
                    '<ul class="pagination">' +
                    '<li class="prev disabled"><a href="#">&larr; ' + oLang.sPrevious + '</a></li>' +
                    '<li class="next disabled"><a href="#">' + oLang.sNext + ' &rarr; </a></li>' +
                    '</ul>'
                );
                var els = $('a', nPaging);
                $(els[0]).bind('click.DT', {
                    action: "previous"
                }, fnClickHandler);
                $(els[1]).bind('click.DT', {
                    action: "next"
                }, fnClickHandler);
            },

            "fnUpdate": function(oSettings, fnDraw) {
                var iListLength = 5;
                var oPaging = oSettings.oInstance.fnPagingInfo();
                var an = oSettings.aanFeatures.p;
                var i, ien, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                if (oPaging.iTotalPages < iListLength) {
                    iStart = 1;
                    iEnd = oPaging.iTotalPages;
                } else if (oPaging.iPage <= iHalf) {
                    iStart = 1;
                    iEnd = iListLength;
                } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                    iStart = oPaging.iTotalPages - iListLength + 1;
                    iEnd = oPaging.iTotalPages;
                } else {
                    iStart = oPaging.iPage - iHalf + 1;
                    iEnd = iStart + iListLength - 1;
                }

                for (i = 0, ien = an.length; i < ien; i++) {
                    // Remove the middle elements
                    $('li:gt(0)', an[i]).filter(':not(:last)').remove();

                    // Add the new list items and their event handlers
                    for (j = iStart; j <= iEnd; j++) {
                        sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                        $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
                            .insertBefore($('li:last', an[i])[0])
                            .bind('click', function(e) {
                                e.preventDefault();
                                oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
                                fnDraw(oSettings);
                            });
                    }

                    // Add / remove disabled classes from the static elements
                    if (oPaging.iPage === 0) {
                        $('li:first', an[i]).addClass('disabled');
                    } else {
                        $('li:first', an[i]).removeClass('disabled');
                    }

                    if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                        $('li:last', an[i]).addClass('disabled');
                    } else {
                        $('li:last', an[i]).removeClass('disabled');
                    }
                }
            }
        }
    });
}


/*
 * TableTools Bootstrap compatibility
 * Required TableTools 2.1+
 */
if ($.fn.DataTable.TableTools) {
    // Set the classes that TableTools uses to something suitable for Bootstrap
    $.extend(true, $.fn.DataTable.TableTools.classes, {
        "container": "DTTT btn-group",
        "buttons": {
            "normal": "btn btn-default",
            "disabled": "disabled"
        },
        "collection": {
            "container": "DTTT_dropdown dropdown-menu",
            "buttons": {
                "normal": "",
                "disabled": "disabled"
            }
        },
        "print": {
            "info": "DTTT_print_info modal"
        },
        "select": {
            "row": "active"
        }
    });

    // Have the collection use a bootstrap compatible dropdown
    $.extend(true, $.fn.DataTable.TableTools.DEFAULTS.oTags, {
        "collection": {
            "container": "ul",
            "button": "li",
            "liner": "a"
        }
    });
}
{
    "emptyTable": "Tabloda herhangi bir veri mevcut deðil",
        "info": "_TOTAL_ kayýttan _START_ - _END_ arasýndaki kayýtlar gösteriliyor",
            "infoEmpty": "Kayýt yok",
                "infoFiltered": "(_MAX_ kayýt içerisinden bulunan)",
                    "infoThousands": ".",
                        "lengthMenu": "Sayfada _MENU_ kayýt göster",
                            "loadingRecords": "Yükleniyor...",
                                "processing": "Ýþleniyor...",
                                    "search": "Ara:",
                                        "zeroRecords": "Eþleþen kayýt bulunamadý",
                                            "paginate": {
        "first": "Ýlk",
            "last": "Son",
                "next": "Sonraki",
                    "previous": "Önceki"
    },
    "aria": {
        "sortAscending": ": artan sütun sýralamasýný aktifleþtir",
            "sortDescending": ": azalan sütun sýralamasýný aktifleþtir"
    },
    "select": {
        "rows": {
            "_": "%d kayýt seçildi",
                "1": "1 kayýt seçildi",
                    "0": "-"
        },
        "0": "-",
            "1": "%d satýr seçildi",
                "2": "-",
                    "_": "%d satýr seçildi",
                        "cells": {
            "1": "1 hücre seçildi",
                "_": "%d hücre seçildi"
        },
        "columns": {
            "1": "1 sütun seçildi",
                "_": "%d sütun seçildi"
        }
    },
    "autoFill": {
        "cancel": "Ýptal",
            "fillHorizontal": "Hücreleri yatay olarak doldur",
                "fillVertical": "Hücreleri dikey olarak doldur",
                    "info": "-",
                        "fill": "Bütün hücreleri <i>%d<\/i> ile doldur"
    },
    "buttons": {
        "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
            "colvis": "Sütun görünürlüðü",
                "colvisRestore": "Görünürlüðü eski haline getir",
                    "copySuccess": {
            "1": "1 satýr panoya kopyalandý",
                "_": "%ds satýr panoya kopyalandý"
        },
        "copyTitle": "Panoya kopyala",
            "csv": "CSV",
                "excel": "Excel",
                    "pageLength": {
            "-1": "Bütün satýrlarý göster",
                "1": "-",
                    "_": "%d satýr göster"
        },
        "pdf": "PDF",
            "print": "Yazdýr",
                "copy": "Kopyala",
                    "copyKeys": "Tablodaki veriyi kopyalamak için CTRL veya u2318 + C tuþlarýna basýnýz. Ýptal etmek için bu mesaja týklayýn veya escape tuþuna basýn."
    },
    "infoPostFix": "-",
        "searchBuilder": {
        "add": "Koþul Ekle",
            "button": {
            "0": "Arama Oluþturucu",
                "_": "Arama Oluþturucu (%d)"
        },
        "condition": "Koþul",
            "conditions": {
            "date": {
                "after": "Sonra",
                    "before": "Önce",
                        "between": "Arasýnda",
                            "empty": "Boþ",
                                "equals": "Eþittir",
                                    "not": "Deðildir",
                                        "notBetween": "Dýþýnda",
                                            "notEmpty": "Dolu"
            },
            "number": {
                "between": "Arasýnda",
                    "empty": "Boþ",
                        "equals": "Eþittir",
                            "gt": "Büyüktür",
                                "gte": "Büyük eþittir",
                                    "lt": "Küçüktür",
                                        "lte": "Küçük eþittir",
                                            "not": "Deðildir",
                                                "notBetween": "Dýþýnda",
                                                    "notEmpty": "Dolu"
            },
            "string": {
                "contains": "Ýçerir",
                    "empty": "Boþ",
                        "endsWith": "Ýle biter",
                            "equals": "Eþittir",
                                "not": "Deðildir",
                                    "notEmpty": "Dolu",
                                        "startsWith": "Ýle baþlar"
            },
            "array": {
                "contains": "Ýçerir",
                    "empty": "Boþ",
                        "equals": "Eþittir",
                            "not": "Deðildir",
                                "notEmpty": "Dolu",
                                    "without": "Hariç"
            }
        },
        "data": "Veri",
            "deleteTitle": "Filtreleme kuralýný silin",
                "leftTitle": "Kriteri dýþarý çýkart",
                    "logicAnd": "ve",
                        "logicOr": "veya",
                            "rightTitle": "Kriteri içeri al",
                                "title": {
            "0": "Arama Oluþturucu",
                "_": "Arama Oluþturucu (%d)"
        },
        "value": "Deðer",
            "clearAll": "Filtreleri Temizle"
    },
    "searchPanes": {
        "clearMessage": "Hepsini Temizle",
            "collapse": {
            "0": "Arama Bölmesi",
                "_": "Arama Bölmesi (%d)"
        },
        "count": "{total}",
            "countFiltered": "{shown}\/{total}",
                "emptyPanes": "Arama Bölmesi yok",
                    "loadMessage": "Arama Bölmeleri yükleniyor ...",
                        "title": "Etkin filtreler - %d"
    },
    "searchPlaceholder": "Ara",
        "thousands": ".",
            "datetime": {
        "amPm": [
            "öö",
            "ös"
        ],
            "hours": "Saat",
                "minutes": "Dakika",
                    "next": "Sonraki",
                        "previous": "Önceki",
                            "seconds": "Saniye",
                                "unknown": "Bilinmeyen"
    },
    "decimal": ",",
        "editor": {
        "close": "Kapat",
            "create": {
            "button": "Yeni",
                "submit": "Kaydet",
                    "title": "Yeni kayýt oluþtur"
        },
        "edit": {
            "button": "Düzenle",
                "submit": "Güncelle",
                    "title": "Kaydý düzenle"
        },
        "error": {
            "system": "Bir sistem hatasý oluþtu (Ayrýntýlý bilgi)"
        },
        "multi": {
            "info": "Seçili kayýtlar bu alanda farklý deðerler içeriyor. Seçili kayýtlarýn hepsinde bu alana ayný deðeri atamak için buraya týklayýn; aksi halde her kayýt bu alanda kendi deðerini koruyacak.",
                "noMulti": "Bu alan bir grup olarak deðil ancak tekil olarak düzenlenebilir.",
                    "restore": "Deðiþiklikleri geri al",
                        "title": "Çoklu deðer"
        },
        "remove": {
            "button": "Sil",
                "confirm": {
                "_": "%d adet kaydý silmek istediðinize emin misiniz?",
                    "1": "Bu kaydý silmek istediðinizden emin misiniz?"
            },
            "submit": "Sil",
                "title": "Kayýtlarý sil"
        }
    }
} 
