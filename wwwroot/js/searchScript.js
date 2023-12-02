$(document).ready(function () {
    $('.circled-search__input').on('input', function () {
        var searchText = $(this).val().toLowerCase();
        $('.item-block').each(function () {
            var title = $(this).find('.name' ).text().toLowerCase();

            if (title.indexOf(searchText) !== -1) {
                $(this).removeClass('hide');
            } else {
                $(this).addClass('hide');
            }
        });
    });
});