$(document).ready(function () {
    $(".circled-search__input").on("focus", function () {
        $(this).closest(".circled-search__block").addClass("focused");
    });

    $(".circled-search__input").on("blur", function () {
        var $input = $(this);
        var $block = $input.closest(".circled-search__block");

        // Проверяем, есть ли текст в input
        if ($input.val().trim() === "") {
            $block.removeClass("focused");
        }
    });
});