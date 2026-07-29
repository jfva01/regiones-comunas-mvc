// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Referenciamos los elementos del DOM para acceder a ellos de manera más limpia en las funciones
const cboRegion = document.getElementById("cboRegion");
const mensajeInicial = document.getElementById("mensajeInicial");
const loadingComunas = document.getElementById("loadingComunas");
const listaComunas = document.getElementById("listaComunas");
const panelEdicion = document.getElementById("panelEdicion");

let txtIdComuna;
let txtNombre;
let txtSuperficie;
let txtPoblacion;
let txtDensidad;
let btnCancelar;
let btnGuardar;

let alerta;

document.addEventListener("DOMContentLoaded", () => {
    cboRegion.addEventListener("change", seleccionarRegion);

    txtIdComuna = document.getElementById("txtIdComuna");
    txtNombre = document.getElementById("txtNombre");
    txtSuperficie = document.getElementById("txtSuperficie");
    txtPoblacion = document.getElementById("txtPoblacion");
    txtDensidad = document.getElementById("txtDensidad");
    btnCancelar = document.getElementById("btnCancelar");
    btnGuardar = document.getElementById("btnGuardar");

    alerta = document.getElementById("alerta");

    btnCancelar.addEventListener("click", cancelarEdicion);
    btnGuardar.addEventListener("click", guardarComuna);
});

async function seleccionarRegion() {
    const idRegion = cboRegion.value;

    if(idRegion === ""){
        listaComunas.classList.add("d-none");
        panelEdicion.classList.add("d-none");
        mensajeInicial.classList.remove("d-none");

        return;
    }

    await cargarComunas(idRegion);
}

function mostrarSpinner(){
    mensajeInicial.classList.add("d-none");
    listaComunas.classList.add("d-none");
    loadingComunas.classList.remove("d-none");
}

function ocultarSpinner(){
    loadingComunas.classList.add("d-none");
}

async function cargarComunas(idRegion) {
    mostrarSpinner();
    try{
        const response = await fetch(`/Home/ObtenerComunasRegion?idRegion=${idRegion}`);

        if(!response.ok)
            throw new Error("Error consultando comunas");
        
        const comunas = await response.json();

        renderizarComunas(comunas);
    }
    catch(error){
        console.error(error);
        mostrarAlerta("No fue posible cargar las comunas.","danger");
    }finally{
        ocultarSpinner();
    }
}

function renderizarComunas(comunas){
    listaComunas.innerHTML = "";

    if(comunas.length === 0){
        listaComunas.innerHTML = `
            <div class="list-group-item text-muted">
                No existen comunas.
            </div>    
        `;

        listaComunas.classList.remove("d-none");

        return;
    }

    const html = comunas.map(comuna =>`
        <div class="card mb-3 shadow-sm">

            <div class="card-body">

                <div class="d-flex justify-content-between align-items-center">
                    <h5>
                        ${comuna.nombre}
                    </h5>
                    <button
                        id="btnEditar" 
                        class="btn btn-primary btn-sm"
                        onclick="editarComuna(${comuna.idComuna}, this)">
                        Editar
                    </button>
                </div>

                <hr>

                <div class="row text-muted">
                    <div class="col-md-4">
                        <strong>Superficie:</strong><br>
                        ${comuna.informacionAdicional.superficie} km²
                    </div>

                    <div class="col-md-4">
                        <strong>Población:</strong><br>
                        ${comuna.informacionAdicional.poblacion.toLocaleString("es-CL")}
                    </div>

                    <div class="col-md-4">
                        <strong>Densidad:</strong><br>
                        ${comuna.informacionAdicional.densidad.toLocaleString("es-CL")}
                    </div>
                </div>
            </div>
        </div>
    `).join("");

    listaComunas.innerHTML = html;

    listaComunas.classList.remove("d-none");
}

function cargarFormulario(comuna) {
    if(!txtIdComuna)
        throw new Error("No existe el elemento txtIdComuna.");

    if(!txtNombre)
        throw new Error("No existe el elemento txtNombre.");

    if(!txtSuperficie)
        throw new Error("No existe el elemento txtSuperficie.");

    if(!txtPoblacion)
        throw new Error("No existe el elemento txtPoblacion.");

    if(!txtDensidad)
        throw new Error("No existe el elemento txtDensidad.");

    txtIdComuna.value = comuna.idComuna;
    txtNombre.value = comuna.nombre;
    txtSuperficie.value = comuna.informacionAdicional.superficie;
    txtPoblacion.value = comuna.informacionAdicional.poblacion;
    txtDensidad.value = comuna.informacionAdicional.densidad;

    panelEdicion.classList.remove("d-none");
}

function cancelarEdicion() {
    panelEdicion.classList.add("d-none");
}

async function editarComuna(idComuna, btnEditar) {
    btnEditar.disabled = true;

    try {
        const response = await fetch(`/Home/ObtenerComuna?idComuna=${idComuna}`);
        if (!response.ok)
            throw new Error("No fue posible obtener la comuna.");
        const comuna = await response.json();
        cargarFormulario(comuna);
    }
    catch (error) {
        console.error(error);
        mostrarAlerta("Ocurrió un error al cargar la comuna.", "danger");
    }finally{
        btnEditar.disabled = false;
    }
}

async function guardarComuna() {
    btnGuardar.disabled = true;

    const comuna = {
        idComuna: Number(txtIdComuna.value),
        nombre: txtNombre.value,
        idRegion: Number(cboRegion.value),
        informacionAdicional: {
            superficie: Number(txtSuperficie.value),
            poblacion: Number(txtPoblacion.value),
            densidad: Number(txtDensidad.value)
        }
    };
    //console.log(comuna);

    try{

    const response = await fetch("/Home/ActualizarComuna", {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(comuna)
    });

    if (!response.ok) {
        throw new Error("No fue posible actualizar la comuna.");
    }

    mostrarAlerta("Comuna actualizada correctamente.", "success");
    await cargarComunas(cboRegion.value);
    panelEdicion.classList.add("d-none");

    }catch (error) {
        console.error(error);
        mostrarAlerta(error.message, "danger");
    }finally{
        btnGuardar.disabled = false;
    }
}
// Función para mostrar mensajes con estilo Bootstrap
function mostrarAlerta(mensaje, tipo) {
    alerta.textContent = mensaje;
    alerta.classList.remove("d-none");
    alerta.classList.remove(
        "alert-success",
        "alert-danger",
        "alert-warning",
        "alert-info"
    );
    alerta.classList.add(`alert-${tipo}`);
    setTimeout(() => {
        alerta.classList.add("d-none");
    }, 3000);
}