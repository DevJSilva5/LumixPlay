// Verifica se está logado (exceto na página de login)
if (!sessionStorage.getItem("logado") && !window.location.href.includes("login.html")) {
    window.location.href = "login.html";
}

// Inserir o Menu Hambúrguer automaticamente nas páginas
document.addEventListener("DOMContentLoaded", function() {
    if (window.location.href.includes("login.html")) return;

    const nav = document.createElement("div");
    nav.className = "menu-container";
    nav.innerHTML = `
        <div id="menuToggle" onclick="toggleMenu()">☰</div>
        <div id="menuItems">
            <a href="balanca.html">⚖️ Balança</a>
            <a href="caixa.html">💰 Caixa</a>
            <a href="saida.html">🚪 Saída</a>
            <a href="#" onclick="logout()" style="color: red;">Logout</a>
        </div>
    `;
    document.body.prepend(nav);
});

function toggleMenu() {
    const items = document.getElementById("menuItems");
    items.style.display = items.style.display === "block" ? "none" : "block";
}

function logout() {
    sessionStorage.removeItem("logado");
    window.location.href = "login.html";
}