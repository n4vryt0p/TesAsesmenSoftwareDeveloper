// get colors array from the string
function getChartColorsArray(chartId) {
    if (document.getElementById(chartId) !== null) {
        var colors = document.getElementById(chartId).getAttribute("data-colors");
        if (colors) {
            colors = JSON.parse(colors);
            return colors.map(function (value) {
                var newValue = value.replace(" ", "");
                if (newValue.indexOf(",") === -1) {
                    var color = getComputedStyle(document.documentElement).getPropertyValue(
                        newValue
                    );
                    if (color) return color;
                    else return newValue;
                } else {
                    var val = value.split(",");
                    if (val.length == 2) {
                        var rgbaColor = getComputedStyle(
                            document.documentElement
                        ).getPropertyValue(val[0]);
                        rgbaColor = "rgba(" + rgbaColor + "," + val[1] + ")";
                        return rgbaColor;
                    } else {
                        return newValue;
                    }
                }
            });
        } else {
            console.warn('data-colors atributes not found on', chartId);
        }
    }
}

// Counter Number
function counterX() {
    var counter = document.querySelectorAll(".counter-value");
    var speed = 250; // The lower the slower
    counter &&
        Array.from(counter).forEach(function (counter_value) {
            function updateCount() {
                var target = +counter_value.getAttribute("data-target");
                var count = +counter_value.innerText;
                var inc = target / speed;
                if (inc < 1) {
                    inc = 1;
                }
                // Check if target is reached
                if (count < target) {
                    // Add inc to count and output in counter_value
                    counter_value.innerText = (count + inc).toFixed(0);
                    // Call function every ms
                    setTimeout(updateCount, 1);
                } else {
                    counter_value.innerText = numberWithCommas(target);
                }
                numberWithCommas(counter_value.innerText);
            }
            updateCount();
        });

    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
}

//  total jobs Charts
var chartRadialbarBasicColors = getChartColorsArray("total_berita");
if (chartRadialbarBasicColors) {
    var options = {
        series: [100],
        chart: {
            type: 'radialBar',
            width: 105,
            sparkline: {
                enabled: true
            }
        },
        dataLabels: {
            enabled: false
        },
        plotOptions: {
            radialBar: {
                hollow: {
                    margin: 0,
                    size: '70%'
                },
                track: {
                    margin: 1
                },
                dataLabels: {
                    show: true,
                    name: {
                        show: false
                    },
                    value: {
                        show: true,
                        fontSize: '16px',
                        fontWeight: 600,
                        offsetY: 8,
                    }
                }
            }
        },
        colors: chartRadialbarBasicColors
    };

    var chartTotNews = new ApexCharts(document.querySelector("#total_berita"), options);
    chartTotNews.render();
}

//  apply jobs Charts
var chartRadialbarBasicColors = getChartColorsArray("korporasi");
if (chartRadialbarBasicColors) {
    var options = {
        series: [window.$dash.korpPercent],
        chart: {
            type: 'radialBar',
            width: 105,
            sparkline: {
                enabled: true
            }
        },
        dataLabels: {
            enabled: false
        },
        plotOptions: {
            radialBar: {
                hollow: {
                    margin: 0,
                    size: '70%'
                },
                track: {
                    margin: 1
                },
                dataLabels: {
                    show: true,
                    name: {
                        show: false
                    },
                    value: {
                        show: true,
                        fontSize: '16px',
                        fontWeight: 600,
                        offsetY: 8,
                    }
                }
            }
        },
        colors: chartRadialbarBasicColors
    };

    var chartKorpNews = new ApexCharts(document.querySelector("#korporasi"), options);
    chartKorpNews.render();
}

//  interview_chart
var chartRadialbarBasicColors = getChartColorsArray("industri");
if (chartRadialbarBasicColors) {
    var options = {
        series: [window.$dash.indPercent],
        chart: {
            type: 'radialBar',
            width: 105,
            sparkline: {
                enabled: true
            }
        },
        dataLabels: {
            enabled: false
        },
        plotOptions: {
            radialBar: {
                hollow: {
                    margin: 0,
                    size: '70%'
                },
                track: {
                    margin: 1
                },
                dataLabels: {
                    show: true,
                    name: {
                        show: false
                    },
                    value: {
                        show: true,
                        fontSize: '16px',
                        fontWeight: 600,
                        offsetY: 8,
                    }
                }
            },
        },
        colors: chartRadialbarBasicColors
    };

    var chartIndNews = new ApexCharts(document.querySelector("#industri"), options);
    chartIndNews.render();
}

//  New jobs Chart
var chartRadialbarBasicColors = getChartColorsArray("kompetitor");
if (chartRadialbarBasicColors) {
    var options = {
        series: [window.$dash.kompPercent],
        chart: {
            type: 'radialBar',
            width: 105,
            sparkline: {
                enabled: true
            }
        },
        dataLabels: {
            enabled: false
        },
        plotOptions: {
            radialBar: {
                hollow: {
                    margin: 0,
                    size: '70%'
                },
                track: {
                    margin: 1
                },
                dataLabels: {
                    show: true,
                    name: {
                        show: false
                    },
                    value: {
                        show: true,
                        fontSize: '16px',
                        fontWeight: 600,
                        offsetY: 8,
                    }
                }
            }
        },
        colors: chartRadialbarBasicColors
    };

    var chartKompNews = new ApexCharts(document.querySelector("#kompetitor"), options);
    chartKompNews.render();
}

// Sales forecast charts
var areachartSalesColors = getChartColorsArray("sentiment-chart");
if (areachartSalesColors) {
    var options = {
        series: [{
            name: 'Positif',
            data: [37]
        }, {
            name: 'Netral',
            data: [12]
        }, {
            name: 'Negatif',
            data: [18]
        }],
        chart: {
            type: 'bar',
            height: 341,
            toolbar: {
                show: false,
            },
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: '65%',
            },
        },
        stroke: {
            show: true,
            width: 5,
            colors: ['transparent']
        },
        xaxis: {
            categories: [''],
            axisTicks: {
                show: false,
                borderType: 'solid',
                color: '#78909C',
                height: 6,
                offsetX: 0,
                offsetY: 0
            },
            title: {
                text: 'Total Sentiment',
                offsetX: 0,
                offsetY: -30,
                style: {
                    color: '#78909C',
                    fontSize: '12px',
                    fontWeight: 400,
                },
            },
        },
        yaxis: {
            tickAmount: 5,
            min: 0
        },
        fill: {
            opacity: 1
        },
        legend: {
            show: true,
            position: 'bottom',
            horizontalAlign: 'center',
            fontWeight: 500,
            offsetX: 0,
            offsetY: -14,
            itemMargin: {
                horizontal: 8,
                vertical: 0
            },
            markers: {
                width: 10,
                height: 10,
            }
        },
        colors: areachartSalesColors
    };
    var chartSentiment = new ApexCharts(document.querySelector("#sentiment-chart"), options);
    chartSentiment.render();
}

function sendDateRange(dr) {
    if (dr.length > 1) {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
        });
        let data = {};
        data.sDate = dr[0];
        data.eDate = dr[1];
        $.ajax({
            url: '../ChangeDash',
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            headers: {
                "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
            },
            success: function (datas) {
                chartKorpNews.updateSeries([datas.korpPercent]);
                chartIndNews.updateSeries([datas.indPercent]);
                chartKompNews.updateSeries([datas.kompPercent]);
                document.getElementById("totCount").setAttribute("data-target", datas.allSum);
                document.getElementById("korpCount").setAttribute("data-target", datas.korpSum);
                document.getElementById("indCount").setAttribute("data-target", datas.indSum);
                document.getElementById("kompCount").setAttribute("data-target", datas.kompSum);
                $.unblockUI();
                counterX();
            },
            error: function (data) {
                $.unblockUI();
                Swal.fire("Server Error!", data.responseText, "error");
            },
        });
    }
    return false;
}

function syncNow() {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
    $.ajax({
        url: '../syncnow',
        type: 'GET',
        headers: {
            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
        },
        success: function () {
            window.location.reload();
        },
        error: function (data) {
            $.unblockUI();
            Swal.fire("Server Error!", data.responseText, "error");
        },
    });
    return false;
};

$(document).ready(function () {
    $('#topGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'id',
            loadUrl: `/ReadTop`,
            onBeforeSend(method, ajaxOptions) {
                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        height: 300,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        remoteOperations: false,
        columns: [
            {
                caption: '#',
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.text(cellInfo.row.rowIndex + 1)
                },
                width: 30
            },
            {
                dataField: 'id',
                visible: false
            },
            {
                dataField: 'title',
                caption: 'Judul'
            },
            {
                dataField: 'url',
                caption: 'Website'
            }
        ],
    })
});