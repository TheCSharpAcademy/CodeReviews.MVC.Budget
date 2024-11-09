export function shortestAngle(index1, index2) {
    var diff = (index2 - index1 + 4) % 4;

    if (diff === 1) {
        return -90;
    } else if (diff === 2) {
        return -180;
    } else if (diff === 3) {
        return 90;
    } else {
        return 0;
    }
}

export function getColor(i) {
    var colors = ['#e6194B', '#3cb44b', '#ffe119', '#4363d8', '#f58231', '#911eb4', '#42d4f4', '#f032e6', '#bfef45', '#fabed4', '#469990', '#dcbeff', '#9A6324', '#fffac8', '#800000', '#aaffc3', '#808000', '#ffd8b1', '#000075'];
    return colors[i%19];
}

export function setupRefocusHandlers() {
    var lastFocus;
    $('.modal').on('show.bs.modal', function () {
        lastFocus = document.activeElement;
    });
    $('.modal').on('hidden.bs.modal', function () {
        if (lastFocus) {
            lastFocus.focus();
        }
    });
}
