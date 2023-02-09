$(() => {

    $("#new-contributor").on('click', function () {
        $("#edit").text(`New Contributor`);
        $("#new-contrib").modal();
    });

    $("#new-simcha").on('click', function () {
        $("#new-sim").modal();
    });

    $(".deposit-button").on('click', function () {
        const button = $(this);
        const name = button.data('name');
        const id = button.data('contribid');
        $(".depositcontribid").val(id);
        $("#deposit-name").text(name);
        $("#deposit").modal();
    });

    $(".edit-contrib").on('click', function () {
        const button = $(this);
        const fname = button.data('first-name');
        const lname = button.data('last-name');
        const cell = button.data('cell');
        const id = button.data('id');
        const include = button.data('always-include');
        const date = button.data('date');

        $(".edit-and-new").find("#edit-id").remove();
        $(".edit-and-new").append(`<input type='hidden' id='edit-id' name='id' value='${id}'/>`);

        $("#contributor_first_name").val(fname);
        $("#contributor_last_name").val(lname);
        $("#contributor_cell_number").val(cell);
        $("#contributor_always_include").prop('checked', include === "True");

        $("#edit").text(`Edit ${fname} ${lname}`);
        $("#initialDepositDiv").hide();

        $("#new-contrib").modal();
        $(".edit-and-new").attr('action', '/Home/EditContributor');
    });


});