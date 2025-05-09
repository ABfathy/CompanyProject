// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// site.js - Enhanced UI interactions for dark theme
document.addEventListener('DOMContentLoaded', function () {
    // Add active class to current nav item
    highlightCurrentNavItem();
    
    // Initialize tooltips
    initTooltips();
    
    // Add animation effects to cards
    animateCards();
    
    // Enhance form validations with animations
    enhanceFormValidation();
    
    // Add subtle hover effects
    addHoverEffects();
});

function highlightCurrentNavItem() {
    // Get current URL path
    const path = window.location.pathname.toLowerCase();
    
    // Find all nav links and check if they match the current path
    document.querySelectorAll('.navbar-nav .nav-link').forEach(link => {
        const href = link.getAttribute('href')?.toLowerCase();
        if (href && (path === href || path.startsWith(href) && href !== '/')) {
            link.classList.add('active');
            link.style.color = 'var(--primary) !important';
        }
    });
}

function initTooltips() {
    // Initialize Bootstrap tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

function animateCards() {
    // Animate cards on page load
    const cards = document.querySelectorAll('.card');
    cards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        
        setTimeout(() => {
            card.style.transition = 'opacity 0.4s ease, transform 0.4s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, 100 * index);
    });
}

function enhanceFormValidation() {
    // Add visual feedback for form validation
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            
            form.classList.add('was-validated');
            
            // Shake effect for invalid inputs
            form.querySelectorAll(':invalid').forEach(input => {
                input.classList.add('shake-animation');
                setTimeout(() => {
                    input.classList.remove('shake-animation');
                }, 500);
            });
        });
        
        // Add validation styles when input changes
        form.querySelectorAll('input, select, textarea').forEach(input => {
            input.addEventListener('blur', function () {
                if (this.checkValidity()) {
                    this.classList.add('is-valid');
                    this.classList.remove('is-invalid');
                } else if (this.value) {
                    this.classList.add('is-invalid');
                    this.classList.remove('is-valid');
                }
            });
        });
    });
}

function addHoverEffects() {
    // Add hover effects to buttons and links
    document.querySelectorAll('.btn').forEach(btn => {
        btn.addEventListener('mouseenter', function() {
            this.style.transition = 'all 0.3s ease';
        });
    });
    
    // Add ripple effect to cards
    document.querySelectorAll('.card').forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
            this.style.boxShadow = '0 10px 20px rgba(0, 0, 0, 0.3)';
            this.style.transition = 'transform 0.3s ease, box-shadow 0.3s ease';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = '0 2px 5px rgba(0, 0, 0, 0.1)';
        });
    });
}

// Add CSS for animations
const style = document.createElement('style');
style.textContent = `
    .shake-animation {
        animation: shake 0.5s cubic-bezier(.36,.07,.19,.97) both;
    }
    
    @keyframes shake {
        10%, 90% { transform: translate3d(-1px, 0, 0); }
        20%, 80% { transform: translate3d(2px, 0, 0); }
        30%, 50%, 70% { transform: translate3d(-4px, 0, 0); }
        40%, 60% { transform: translate3d(4px, 0, 0); }
    }
    
    .nav-link.active {
        color: var(--primary) !important;
        font-weight: 500;
        position: relative;
    }
    
    .nav-link.active::after {
        content: '';
        position: absolute;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%);
        width: 20px;
        height: 2px;
        background-color: var(--primary);
        border-radius: 2px;
    }
`;
document.head.appendChild(style);
