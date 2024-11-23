document.addEventListener("DOMContentLoaded", () => {
    // Last 7 Days Statistics Chart
    /**
     * @type {HTMLCanvasElement}
     */
    const dateChartElement = document.getElementById('last-7-days-statistics-chart');

    /**
     * @type {string[]}
     */
    let dateChartLabels = JSON.parse(dateChartElement.getAttribute("chart-labels"));

    /**
     * @type {number[]}
     */
    let dateChartAccessCounts = JSON.parse(dateChartElement.getAttribute("chart-access-count"));

    /**
     * @type {number[]}
     */
    let dateChartGuessCounts = JSON.parse(dateChartElement.getAttribute("chart-guess-count"));

    const dateChart = new ApexCharts(dateChartElement, {
        chart: {
            type: "area",
            height: "300px",
            toolbar: {
                show: false
            },
            sparkline: {
                enabled: false
            },
        },
        dataLabels: {
            enabled: false
        },
        series: [
            {
                name: "Lượt truy cập",
                data: dateChartAccessCounts
            },
            {
                name: "Lượt khách",
                data: dateChartGuessCounts
            }
        ],
        fill: {
            type: "gradient",
            gradient: {
                opacityFrom: 0.4,
                opacityTo: 0,
            }
        },
        markers: {
            size: 7,
            shape: "circle",
            hover: {
                size: 10,
            }
        },
        stroke: {
            show: true,
        },
        xaxis: {
            categories: dateChartLabels.map(label => label.split(", ")),
            labels: {
                show: true,
                rotate: -45
            },
            axisBorder: {
                show: false,
            },
            axisTicks: {
                show: false
            },
            tooltip: {
                enabled: false
            }
        },
        yaxis: {
            tickAmount: 5,
            labels: {
                show: true
            },
            axisBorder: {
                show: false,
            },
            axisTicks: {
                show: false
            }
        },
        grid: {
            show: false,
            padding: {
                top: 40,
                left: 20,
            }
        },
        legend: {
            show: true,
            position: "top"
        },
        tooltip: {
            shared: true,
            fixed: {
                enabled: false,
                position: 'topRight',
                offsetX: -10,
                offsetY: 0,
            },
        },
    });

    
    // Hour Range Statistics Chart
    /**
     * @type {HTMLCanvasElement}
     */
    const hourChartElement = document.getElementById("hour-range-statistics-chart");

    /**
     * @type {string[]}
     */
    let hourChartLabels = JSON.parse(hourChartElement.getAttribute("chart-labels"));

    /**
     * @type {number[]}
     */
    let hourChartAccessCounts = JSON.parse(hourChartElement.getAttribute("chart-access-count"));

    /**
     * @type {number[]}
     */
    let hourChartGuessCounts = JSON.parse(hourChartElement.getAttribute("chart-guess-count"));

    const hourChart = new ApexCharts(hourChartElement, {
        chart: {
            type: 'bar',
            height: "250px",
            toolbar: {
                show: false
            },
        },
        dataLabels: {
            enabled: false
        },
        plotOptions: {
            bar: {
                horizontal: false,
                borderRadius: 5,
                borderRadiusApplication: 'end',
                borderRadiusWhenStacked: 'last',
            }
        },
        series: [
            {
                name: 'Lượt truy cập',
                data: hourChartAccessCounts
            },
            {
                name: "Lượt khách",
                data: hourChartGuessCounts
            }
        ],
        xaxis: {
            categories: hourChartLabels.map(label => label.split(" ")),
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false,
            },
            labels: {
                show: true,
            }
        },
        yaxis: {
            tickAmount: 5,
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false,
            },
            labels: {
                show: true,
            }
        },
        legend: {
            show: true,
            position: "top"
        },
        tooltip: {
            shared: false,
        },
        grid: {
            show: false,
            padding: {
            }
        },
    });


    // Device Statistics Charts
    

    
    // Hour Range Statistics Chart
    /**
     * @type {HTMLCanvasElement}
     */
    const deviceChartElement = document.getElementById("device-statistics-chart");

    /**
     * @type {string[]}
     */
    let deviceChartLabels = JSON.parse(deviceChartElement.getAttribute("chart-device-names"));

    /**
     * @type {number[]}
     */
    let deviceAccessCount = JSON.parse(deviceChartElement.getAttribute("chart-access-count"));

    const deviceChart = new ApexCharts(deviceChartElement, {
        chart: {
            type: 'pie',
            height: "250px",
            toolbar: {
                show: false
            },
        },
        labels: deviceChartLabels,
        series: deviceAccessCount,
        legend: {
            show: true,
            position: "top"
        },
        grid: {
            show: true,
            padding: {
                bottom: -25
            }
        },
    });
    hourChart.render();
    dateChart.render();
    deviceChart.render();
});

