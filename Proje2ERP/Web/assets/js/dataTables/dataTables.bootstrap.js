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
    "emptyTable": "Tabloda herhangi bir veri mevcut de�il",
        "info": "_TOTAL_ kay�ttan _START_ - _END_ aras�ndaki kay�tlar g�steriliyor",
            "infoEmpty": "Kay�t yok",
                "infoFiltered": "(_MAX_ kay�t i�erisinden bulunan)",
                    "infoThousands": ".",
                        "lengthMenu": "Sayfada _MENU_ kay�t g�ster",
                            "loadingRecords": "Y�kleniyor...",
                                "processing": "��leniyor...",
                                    "search": "Ara:",
                                        "zeroRecords": "E�le�en kay�t bulunamad�",
                                            "paginate": {
        "first": "�lk",
            "last": "Son",
                "next": "Sonraki",
                    "previous": "�nceki"
    },
    "aria": {
        "sortAscending": ": artan s�tun s�ralamas�n� aktifle�tir",
            "sortDescending": ": azalan s�tun s�ralamas�n� aktifle�tir"
    },
    "select": {
        "rows": {
            "_": "%d kay�t se�ildi",
                "1": "1 kay�t se�ildi",
                    "0": "-"
        },
        "0": "-",
            "1": "%d sat�r se�ildi",
                "2": "-",
                    "_": "%d sat�r se�ildi",
                        "cells": {
            "1": "1 h�cre se�ildi",
                "_": "%d h�cre se�ildi"
        },
        "columns": {
            "1": "1 s�tun se�ildi",
                "_": "%d s�tun se�ildi"
        }
    },
    "autoFill": {
        "cancel": "�ptal",
            "fillHorizontal": "H�creleri yatay olarak doldur",
                "fillVertical": "H�creleri dikey olarak doldur",
                    "info": "-",
                        "fill": "B�t�n h�creleri <i>%d<\/i> ile doldur"
    },
    "buttons": {
        "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
            "colvis": "S�tun g�r�n�rl���",
                "colvisRestore": "G�r�n�rl��� eski haline getir",
                    "copySuccess": {
            "1": "1 sat�r panoya kopyaland�",
                "_": "%ds sat�r panoya kopyaland�"
        },
        "copyTitle": "Panoya kopyala",
            "csv": "CSV",
                "excel": "Excel",
                    "pageLength": {
            "-1": "B�t�n sat�rlar� g�ster",
                "1": "-",
                    "_": "%d sat�r g�ster"
        },
        "pdf": "PDF",
            "print": "Yazd�r",
                "copy": "Kopyala",
                    "copyKeys": "Tablodaki veriyi kopyalamak i�in CTRL veya u2318 + C tu�lar�na bas�n�z. �ptal etmek i�in bu mesaja t�klay�n veya escape tu�una bas�n."
    },
    "infoPostFix": "-",
        "searchBuilder": {
        "add": "Ko�ul Ekle",
            "button": {
            "0": "Arama Olu�turucu",
                "_": "Arama Olu�turucu (%d)"
        },
        "condition": "Ko�ul",
            "conditions": {
            "date": {
                "after": "Sonra",
                    "before": "�nce",
                        "between": "Aras�nda",
                            "empty": "Bo�",
                                "equals": "E�ittir",
                                    "not": "De�ildir",
                                        "notBetween": "D���nda",
                                            "notEmpty": "Dolu"
            },
            "number": {
                "between": "Aras�nda",
                    "empty": "Bo�",
                        "equals": "E�ittir",
                            "gt": "B�y�kt�r",
                                "gte": "B�y�k e�ittir",
                                    "lt": "K���kt�r",
                                        "lte": "K���k e�ittir",
                                            "not": "De�ildir",
                                                "notBetween": "D���nda",
                                                    "notEmpty": "Dolu"
            },
            "string": {
                "contains": "��erir",
                    "empty": "Bo�",
                        "endsWith": "�le biter",
                            "equals": "E�ittir",
                                "not": "De�ildir",
                                    "notEmpty": "Dolu",
                                        "startsWith": "�le ba�lar"
            },
            "array": {
                "contains": "��erir",
                    "empty": "Bo�",
                        "equals": "E�ittir",
                            "not": "De�ildir",
                                "notEmpty": "Dolu",
                                    "without": "Hari�"
            }
        },
        "data": "Veri",
            "deleteTitle": "Filtreleme kural�n� silin",
                "leftTitle": "Kriteri d��ar� ��kart",
                    "logicAnd": "ve",
                        "logicOr": "veya",
                            "rightTitle": "Kriteri i�eri al",
                                "title": {
            "0": "Arama Olu�turucu",
                "_": "Arama Olu�turucu (%d)"
        },
        "value": "De�er",
            "clearAll": "Filtreleri Temizle"
    },
    "searchPanes": {
        "clearMessage": "Hepsini Temizle",
            "collapse": {
            "0": "Arama B�lmesi",
                "_": "Arama B�lmesi (%d)"
        },
        "count": "{total}",
            "countFiltered": "{shown}\/{total}",
                "emptyPanes": "Arama B�lmesi yok",
                    "loadMessage": "Arama B�lmeleri y�kleniyor ...",
                        "title": "Etkin filtreler - %d"
    },
    "searchPlaceholder": "Ara",
        "thousands": ".",
            "datetime": {
        "amPm": [
            "��",
            "�s"
        ],
            "hours": "Saat",
                "minutes": "Dakika",
                    "next": "Sonraki",
                        "previous": "�nceki",
                            "seconds": "Saniye",
                                "unknown": "Bilinmeyen"
    },
    "decimal": ",",
        "editor": {
        "close": "Kapat",
            "create": {
            "button": "Yeni",
                "submit": "Kaydet",
                    "title": "Yeni kay�t olu�tur"
        },
        "edit": {
            "button": "D�zenle",
                "submit": "G�ncelle",
                    "title": "Kayd� d�zenle"
        },
        "error": {
            "system": "Bir sistem hatas� olu�tu (Ayr�nt�l� bilgi)"
        },
        "multi": {
            "info": "Se�ili kay�tlar bu alanda farkl� de�erler i�eriyor. Se�ili kay�tlar�n hepsinde bu alana ayn� de�eri atamak i�in buraya t�klay�n; aksi halde her kay�t bu alanda kendi de�erini koruyacak.",
                "noMulti": "Bu alan bir grup olarak de�il ancak tekil olarak d�zenlenebilir.",
                    "restore": "De�i�iklikleri geri al",
                        "title": "�oklu de�er"
        },
        "remove": {
            "button": "Sil",
                "confirm": {
                "_": "%d adet kayd� silmek istedi�inize emin misiniz?",
                    "1": "Bu kayd� silmek istedi�inizden emin misiniz?"
            },
            "submit": "Sil",
                "title": "Kay�tlar� sil"
        }
    }
} 
