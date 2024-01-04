$(document).ready(() => {

    let id = $('#EventId').val();
    let index = 1;

    $.validator.addMethod("pattern", function (value, element, params) {
        var pattern = new RegExp(params);
        return this.optional(element) || pattern.test(value);
    }, "Некорректный формат");

    $(document).on('click', '.measureTitle', function () {
        $(this).next(".itemBlock").slideToggle();
    });
    $(document).on('click', '.deleteButton', function () {
        $(this).closest('.itemBlock').remove();
    });
    $('#addBtn').click(() => {
        var lastBlock = $('#holder').find('.itemBlock:last');
        if ($('#applicationForm').valid()) {
            var secondName = lastBlock.find('input[name$=".SecondName"]').val();
            var name = lastBlock.find('input[name$=".Name"]').val();
            $('p.measureTitle:last').text(secondName + ' ' + name);

            $.ajax({
                url: "/EventApplications/GetParticipantEntry?Id=" + id + "&Index=" + index,
                type: "GET",
                success: function (partialView) {
                    $("#holder").append(partialView);
                    index++;
                    $('#holder').find('.itemBlock:last input[data-val="true"]').each(function () {
                        $(this).rules('add', {
                            required: true,
                            pattern: $(this).data('val-regex-pattern'),
                            messages: {
                                required: $(this).attr('data-val-required'),
                                pattern: $(this).data('val-regex')
                            }
                        });
                    });

                    $('#holder').find('.itemBlock:last select[data-val="true"]').each(function () {
                        $(this).rules('add', {
                            required: true,
                            messages: {
                                required: $(this).attr('data-val-required')
                            }
                        });
                    });
                }
            });
        }
    });

});