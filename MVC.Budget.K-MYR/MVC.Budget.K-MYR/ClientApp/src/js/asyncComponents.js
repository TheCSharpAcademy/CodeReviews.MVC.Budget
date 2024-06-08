export async function getCountrySelect(id) {
    const { default: _ } = await import(/* webpackChunkName: "countrySelect" */ 'country-select-js');
    return $(id).countrySelect({
        defaultCountry: window.userLocale.region.toLowerCase(),
        preferredCountries: ["at", "us"],
        responsiveDropdown: true
    });
}

export async function importChartDefaults() {
    const { Chart, LinearScale, Legend, Tooltip, Colors } = await import(/* webpackChunkName: "chartJS" */'chart.js');

    Chart.register(
        LinearScale, Legend, Tooltip, Colors,
        {
            id: "emptypiechart",
            beforeInit: function (chart) {
                chart.data.datasets[0].backgroundColor.push('#d2dee2');
                chart.data.datasets[0].data.push(Number.MIN_VALUE);
            }
        }
    );

    Chart.defaults.color = '#ffffff';
    Chart.defaults.scales.linear.min = 0;
    Chart.defaults.plugins.legend.labels.filter = (item) => item.text !== undefined;
    Chart.defaults.plugins.tooltip.filter = (item) => item.label !== "";
}

export async function importBootstrapCollapses() {
    const { Collapse } = await import(/* webpackChunkName: "bootstrap-collapse" */'bootstrap');
    let collapseElements = document.querySelectorAll('.collapse')
    let collapses = [...collapseElements].map(collapseElement => new Collapse(collapseElement, { toggle: false }))

    return collapses;
}

export async function importBootstrapModals() {
    const { Modal } = await import(/* webpackChunkName: "bootstrap-modals" */'bootstrap');
    let modalElements = document.querySelectorAll('.modal')
    let modals = [...modalElements].map(modalElement => new Modal(modalElement, { toggle: false }))

    return modals;
}

export async function getDatePicker(id, mode) {
    const { default: _ } = await import(/* webpackChunkName: "datepicker" */'bootstrap-datepicker');

    switch (mode) {
        case "month":
            return $(id).datepicker({
                format: 'MM yyyy',
                startView: 'months',
                minViewMode: 'months',
                autoclose: true
            })
        default:
            return $(id).datepicker({
                format: 'yyyy',
                minViewMode: 'years',
                autoclose: true
            });
    }
}