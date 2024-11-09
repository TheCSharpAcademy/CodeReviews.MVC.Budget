export async function getCountrySelect(id) {
    try {
        const { default: _ } = await import(/* webpackChunkName: "countrySelect" */ 'country-select-js');         
        return $(id).countrySelect({
            defaultCountry: window.userLocale.region.toLowerCase(),
            preferredCountries: ["at", "us"],
            excludeCountries: []
        });
    } catch (error) {
        console.error('Error loading Country Select:', error);
        throw error;
    }
}

export async function importChartDefaults() {
    try {
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

        Chart.defaults.normalized = true;
        Chart.defaults.color = '#ffffff';
        Chart.defaults.scales.linear.min = 0;
        Chart.defaults.plugins.legend.labels.filter = (item) => item.text !== undefined;
        Chart.defaults.plugins.tooltip.filter = (item) => item.label !== ""; 
        Chart.defaults.layout.padding = 2;
    } catch (error) {
        console.error('Error loading Chart.js defaults:', error);
        throw error; 
    }
}

export async function importBootstrapCollapses() {
    try {
        const Collapse = (await import(/* webpackChunkName: "bootstrap-collapses" */'bootstrap/js/dist/collapse.js')).default;
        let collapseElements = document.querySelectorAll('.collapse')
        let collapses = [...collapseElements].map(collapseElement => new Collapse(collapseElement, { toggle: false }))

        return collapses;
    } catch (error) {
        console.error('Error loading Bootstrap collapses:', error);
        throw error;
    }
}

export async function importBootstrapModals() {
    try {
        const Modal = (await import(/* webpackChunkName: "bootstrap-modals" */'bootstrap/js/dist/modal')).default;
        let modalElements = document.querySelectorAll('.modal')
        let modals = [...modalElements].map(modalElement => new Modal(modalElement, { toggle: false }))

        return modals;
    } catch (error) {
        console.error('Error loading Bootstrap modals:', error);
        throw error; 
    }
}

export async function getDatePicker(id, mode) {
    try {
        const { default: _ } = await import(/* webpackChunkName: "datepicker" */'bootstrap-datepicker');

        switch (mode) {
            case "month":
                return $(id).datepicker({
                    format: 'MM yyyy',
                    startView: 'months',
                    minViewMode: 'months',
                    autoclose: true
                });
            default:
                return $(id).datepicker({
                    format: 'yyyy',
                    minViewMode: 'years',
                    autoclose: true
                });
        }
    } catch (error) {
        console.error('Error loading Datepicker:', error);
        throw error; 
    }    
}