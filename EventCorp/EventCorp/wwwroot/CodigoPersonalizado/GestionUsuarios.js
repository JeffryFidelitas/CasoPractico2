$(document).ready(function () {
    // Al hacer clic en el botón de editar, carga los datos del usuario en el modal
    $('.btn-editar').click(function () {
        const userId = $(this).data('id');

        $.ajax({
            url: '/Usuario/EditarUsuario/' + userId,
            type: 'GET',
            success: function (data) {
                $('#editarId').val(data.id);
                $('#editarNombreCompleto').val(data.nombreCompleto);
                $('#editarNombreUsuario').val(data.nombreUsuario);
                $('#editarCorreo').val(data.correo);
                $('#editarTelefono').val(data.telefono);
                $('#editarRol').val(data.rol);
                cargarRoles(data.rol);

                var modal = new bootstrap.Modal(document.getElementById('modalEditarUsuario'));
                modal.show();
            },
            error: function () {
                showToast('Error al obtener los datos del usuario.', 'danger');
            }
        });
    });

    function cargarRoles(rolSeleccionado) {
        $.ajax({
            url: '/Usuario/ObtenerRoles',
            type: 'GET',
            success: function (roles) {
                var $select = $('#editarRol');
                $select.empty();
                $.each(roles, function (index, role) {
                    var option = $('<option></option>')
                        .val(role.id)
                        .text(role.name);
                    if (role.id == rolSeleccionado) {
                        option.prop('selected', true);
                    }
                    $select.append(option);
                });
            },
            error: function () {
                showToast('Error al obtener los roles.', 'danger');
            }
        });
    }

    // Enviar formulario
    $('#formEditarUsuario').submit(function (e) {
        e.preventDefault();

        var formData = {
            Id: $('#editarId').val(),
            NombreCompleto: $('#editarNombreCompleto').val(),
            NombreUsuario: $('#editarNombreUsuario').val(),
            Correo: $('#editarCorreo').val(),
            Telefono: $('#editarTelefono').val(),
            Rol: $('#editarRol').val()
        };

        $.ajax({
            url: '/Usuario/ActualizarUsuario',
            type: 'POST',
            data: formData,
            success: function (response) {
                showToast(response.mensaje, 'success');
                setTimeout(() => location.reload(), 2000); // Espera 2 segundos antes de recargar
            },
            error: function (response) {
                var errorMessage = 'Error al actualizar el usuario: ' + response.responseJSON.mensaje + '. ' + response.responseJSON.errores;
                showToast(errorMessage, 'danger');
            }
        });
    });

    // Función para mostrar toast
    function showToast(message, type) {
        const toastElement = document.getElementById('toastMensaje');
        const toastTitle = document.getElementById('toastTitle');
        const toastMessage = document.getElementById('toastMessage');
        const toastIcon = document.getElementById('toastIcon');

        // Reset classes
        toastElement.classList.remove('bg-success', 'bg-danger');
        toastIcon.className = ''; // Quita iconos anteriores

        if (type === 'success') {
            toastElement.classList.add('bg-success');
            toastIcon.classList.add('fa-solid', 'fa-circle-check');
            toastTitle.innerText = ' ¡Éxito! ';
        } else if (type === 'danger') {
            toastElement.classList.add('bg-danger');
            toastIcon.classList.add('fa-solid', 'fa-triangle-exclamation');
            toastTitle.innerText = ' Error: ';
        }

        toastMessage.innerText = message;

        const toast = new bootstrap.Toast(toastElement);
        toast.show();
    }

});
